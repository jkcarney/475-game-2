using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Food foodName;
    public PhysicMaterial foodPhysics;
    public Material foodMaterial;

    public bool isGarbage = false;

    void Start()
    {
        GetComponent<MeshRenderer>().material = foodMaterial;
        GetComponent<BoxCollider>().material = foodPhysics;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I, " + this.gameObject.name + " am colliding with " + collision.gameObject.name);
    }
    
    public enum Food 
    {
        TopBun,
        BottomBun,
        Patty,
        Lettuce,
        Cheese
    }
}
