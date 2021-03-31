using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderDetection : MonoBehaviour
{
    [Range(-150.0f, 0.0f)]
    public float stickiness;

    public float centerBalance;

    public AudioSource leavingFoodSFX;

    public GameObject goodNextItemParticle;
    
    private OrderManager om;

    private List<string> isOnPan;

    private BoxCollider orderCollider;

    private List<Collider> triggerList = new List<Collider>();

    private Vector3 orderDetectorTriggerCenterDefault = new Vector3(0.0f, -1.2f, 0.0f);
    private Vector3 orderDetectorTriggerSizeDefault = new Vector3(1.0f, 0.75f, 1.0f);
    
    void Start()
    {
        // Get the order manager
        om = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        isOnPan = new List<string>();
        orderCollider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        // Null check
        if(triggerList.Count > 0)
        {
            // If an object is on the pan, then apply a downwards force so upwards movement does not send it
            // flying as easily
            foreach(Collider c in triggerList)
            {
                c.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, stickiness, 0.0f), ForceMode.Acceleration);

                c.gameObject.transform.position = Vector3.MoveTowards(
                    (c.gameObject.transform.position), 
                    new Vector3(transform.position.x, c.gameObject.transform.position.y, transform.position.z), 
                    centerBalance * Time.deltaTime
                    );
            }
        }
    }

    // When the collider detects a new object, add it to the "isOnPan" list
    void OnTriggerEnter(Collider other)
    {
        // If the list of triggers doesnt contain this one, add it
        // the half sphere part is a fix for a bug with the top bun
        if(!triggerList.Contains(other) && other.name != "half_sphere")
        {
            triggerList.Add(other);
            string nextFood = other.GetComponent<FoodItem>().foodName.ToString();
            if(other.GetComponent<FoodItem>().isGarbage)
            {
                nextFood = "GARBAGE";
            }
            // If the item added matches, play sound w/ particle
            if(nextFood == om.GetNextExpectedItem())
            {
                /*
                if(om.GetNextExpectedItem() == OrderManager.reversedCurrentOrder[3])
                {
                    OrderManager.ingredientFour.color = Color.green;
                }
                if(om.GetNextExpectedItem() == OrderManager.reversedCurrentOrder[2])
                {
                    OrderManager.ingredientThree.color = Color.green;
                }
                if(om.GetNextExpectedItem() == OrderManager.reversedCurrentOrder[1])
                {
                    OrderManager.ingredientTwo.color = Color.green;
                }
                if(om.GetNextExpectedItem() == OrderManager.reversedCurrentOrder[0])
                {
                    OrderManager.ingredientOne.color = Color.green;
                }
                */
                GameObject p = Instantiate(goodNextItemParticle, other.transform.position, Quaternion.identity);
                p.GetComponent<AudioSource>().Play();
                Destroy(p, 2.0f);
            }
            /*
            else if (nextFood != om.GetNextExpectedItem()){
                    OrderManager.ingredientFour.color = Color.red;
                    OrderManager.ingredientThree.color = Color.red;
                    OrderManager.ingredientTwo.color = Color.red;
                    OrderManager.ingredientOne.color = Color.red;
            }*/
            isOnPan.Add(nextFood);
            // Expand size of collider to detect new ingredients 
            orderCollider.size = new Vector3(orderCollider.size.x, orderCollider.size.y + 0.4f, orderCollider.size.z);
            orderCollider.center = new Vector3(orderCollider.center.x, orderCollider.center.y - 0.2f, orderCollider.center.z);
        }
    }

    // If colliders leave, remove it from the isOnPan list
    void OnTriggerExit(Collider other)
    {
        
        string getFood = other.GetComponent<FoodItem>().foodName.ToString();
        /*
        if(getFood == OrderManager.reversedCurrentOrder[3]){
            OrderManager.ingredientFour.color = Color.white;
            }
            if(getFood == OrderManager.reversedCurrentOrder[2]){
                    OrderManager.ingredientThree.color = Color.white;
            }
            if(getFood == OrderManager.reversedCurrentOrder[1]){
                    OrderManager.ingredientTwo.color = Color.white;
            }
            if(getFood == OrderManager.reversedCurrentOrder[0]){
                    OrderManager.ingredientOne.color = Color.white;
            }
            */
           // If the list of triggers contains this collider, remove it
        if(triggerList.Contains(other))
        {
            triggerList.Remove(other);
            string foodGone = other.GetComponent<FoodItem>().foodName.ToString();
            if(other.GetComponent<FoodItem>().isGarbage)
            {
                foodGone = "GARBAGE";
            }

            isOnPan.Reverse();
            isOnPan.Remove(foodGone);
            isOnPan.Reverse();
            
            // Shrink size of collider appropriately
            orderCollider.size = new Vector3(orderCollider.size.x, orderCollider.size.y - 0.4f, orderCollider.size.z);
            orderCollider.center = new Vector3(orderCollider.center.x, orderCollider.center.y + 0.2f, orderCollider.center.z);
            leavingFoodSFX.pitch = Random.Range(0.75f, 1.25f);
            leavingFoodSFX.Play();
        }
    }

    public void ResetOrderDetectorTrigger()
    {
        orderCollider.center = orderDetectorTriggerCenterDefault;
        orderCollider.size = orderDetectorTriggerSizeDefault;
    }

    public List<string> GetWhatsOnPan()
    {
        return isOnPan;
    }

    public void RemoveAllPanItems()
    {
        //OrderManager.PrintOrder.color = Color.white;
        foreach(Collider c in triggerList)
            Destroy(c.gameObject);
        triggerList.Clear();
        isOnPan.Clear();
        /*
        OrderManager.ingredientFour.color = Color.white;
        OrderManager.ingredientThree.color = Color.white;
        OrderManager.ingredientTwo.color = Color.white;
        OrderManager.ingredientOne.color = Color.white;
        */
    }
}
