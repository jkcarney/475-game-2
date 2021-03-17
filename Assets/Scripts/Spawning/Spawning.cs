using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    //introduce all ingredients
    public GameObject TopBun;
    public GameObject BottomBun;
    public GameObject Patty;
    //public GameObject Lettuce;
    public GameObject Cheese;
    private GameObject food;

    //variables for spawning food
    public float spawnRate = 1f; //change the speed that food spawns
    private float lastSpawned;

    //spawns food at Random X between -10 and 10, 30 Y, and -25 Z
    void Spawn(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(Random.Range(-10.0f, 10.0f), 30.0f, -25.0f), Quaternion.identity);
    }
    
    //destroys food on collision with the DestroyPlane
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }


    void Update()
    {
        //randomly selecting which ingredient to spawn
        var randomNumber = Random.Range(1, 6);
        if (randomNumber == 1) food = TopBun;
        else if (randomNumber == 2) food = BottomBun;
        else if (randomNumber == 3) food = Patty;
        //else if (randomNumber == 4) food = Lettuce;
        else if (randomNumber == 5) food = Cheese;


        //process for creating the delay in spawning the food
        if (Time.time > lastSpawned + spawnRate)
        {
            Spawn(food);
            lastSpawned = Time.time;
        }
        

    }
}
