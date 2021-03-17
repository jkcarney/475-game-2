using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDetection : MonoBehaviour
{
    [Range(-150.0f, 0.0f)]
    public float stickiness;
    
    private OrderManager om;

    private List<string> isOnPan;

    private BoxCollider orderCollider;

    private List<Collider> triggerList = new List<Collider>();
    
    void Start()
    {
        // Get the order manager
        om = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        isOnPan = new List<string>();
        orderCollider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        foreach(Collider c in triggerList)
        {
            c.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, stickiness, 0.0f), ForceMode.Acceleration);
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
            isOnPan.Add(nextFood);
            // Expand size of collider to detect new ingredients 
            orderCollider.size = new Vector3(orderCollider.size.x, orderCollider.size.y + 0.4f, orderCollider.size.z);
            orderCollider.center = new Vector3(orderCollider.center.x, orderCollider.center.y - 0.2f, orderCollider.center.z);
        }
    }

    // If colliders leave, remove it from the isOnPan list
    void OnTriggerExit(Collider other)
    {
        // If the list of triggers contains this collider, remove it
        if(triggerList.Contains(other))
        {
            triggerList.Remove(other);
            string foodGone = other.GetComponent<FoodItem>().foodName.ToString();
            isOnPan.Remove(foodGone);
            // Shrink size of collider appropriately
            orderCollider.size = new Vector3(orderCollider.size.x, orderCollider.size.y - 0.4f, orderCollider.size.z);
            orderCollider.center = new Vector3(orderCollider.center.x, orderCollider.center.y + 0.2f, orderCollider.center.z);
        }
    }

    public List<string> GetWhatsOnPan()
    {
        return isOnPan;
    }

    public void RemoveAllPanItems()
    {
        foreach(Collider c in triggerList)
            Destroy(c.gameObject);
    }
}
