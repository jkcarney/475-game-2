using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Food foodName;
    public PhysicMaterial foodPhysics;
    public Material foodMaterial;

    public bool isGarbage = false;

    public bool trackpadMode = false;

    private Rigidbody rigidbody;

    void Start()
    {
        if(foodMaterial != null && foodPhysics != null)
        {
            GetComponent<MeshRenderer>().material = foodMaterial;
            GetComponent<BoxCollider>().material = foodPhysics;
        }
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(rigidbody != null && rigidbody.velocity.magnitude > 30f && trackpadMode)
        {
            Debug.Log("EXCEEDED!");
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, 30f);
        }
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
