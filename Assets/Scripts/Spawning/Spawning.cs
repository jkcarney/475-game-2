using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    public GameObject TopBun;
    public GameObject BottomBun;
    public GameObject Patty;
    //public GameObject Lettuce;
    public GameObject Cheese;
    private GameObject food;

    public float spawnRate = 1f;
    private float lastSpawned;

    void Start()
    {

    }

    void Spawn(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(Random.Range(-10.0f, 10.0f), 30.0f, -25.0f), Quaternion.identity);
    }
    
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }


    void Update()
    {
        var randomNumber = Random.Range(1, 6);
        if (randomNumber == 1) food = TopBun;
        else if (randomNumber == 2) food = BottomBun;
        else if (randomNumber == 3) food = Patty;
        //else if (randomNumber == 4) food = Lettuce;
        else if (randomNumber == 5) food = Cheese;

        //Invoke("DeactivateText", 10.0f); 
        //Invoke("Spawn(food)", 1);
        if (Time.time > lastSpawned + spawnRate)
        {
            Spawn(food);
            lastSpawned = Time.time;
        }
        

    }
}
