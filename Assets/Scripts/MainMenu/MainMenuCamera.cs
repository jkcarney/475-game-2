using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    // Our Level prefabs are stored in this array
    public GameObject[] levelLoad;

    public GameObject titleObj;
    public GameObject menuObj;

    public float rotationSpeed;

    public float gravityPower;

    private Vector3 target_rotation;
    private bool isFacingSky = true;
    
    // Start current level index at -1 for the main menu
    private int currentLevelIndex = -1;

    void Start()
    {
        Physics.gravity = new Vector3(0.0f, gravityPower, 0.0f);
    }

    void Update()
    {
        // Rotate towards the desired rotation which gets updated in the Rotate function
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target_rotation, Time.deltaTime * rotationSpeed);
    }

    public void Rotate180Degrees()
    {
        if(isFacingSky)
        {
            target_rotation = new Vector3(0.0f, 180.0f, 0.0f);
            isFacingSky = false;
        }
        else
        {
            target_rotation = new Vector3(0.0f, 0.0f, 0.0f);
            isFacingSky = true;
        }
    }

    // Finds the level in the scene at the specified index and destroy it
    public void DestroyLevelAtGivenIndex(int index)
    {
        string levelToDestroy = "Level" + index.ToString() + "(Clone)";
        GameObject objectToDestroy = GameObject.Find(levelToDestroy);
        Destroy(objectToDestroy);
    }

    // Instaniates the level in the scene at the specified index
    public void SpawnLevelAtGivenIndex(int index)
    {
        Instantiate(levelLoad[index], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }

    // Called by the >> button to step through the levels array
    public void NextDifficulty()
    {
        // We start at -1, so don't try to find something at the -1 index
        if(currentLevelIndex != -1)
        {
            DestroyLevelAtGivenIndex(currentLevelIndex);
        }

        // incr the current level index
        ++currentLevelIndex;

        // if we've gone over the amount of indexes in the array, set it back to 0 so it's circular
        if(currentLevelIndex == levelLoad.Length)
        {
            currentLevelIndex = 0;
        }
        SpawnLevelAtGivenIndex(currentLevelIndex);
    }

    public void PreviousDifficulty()
    {
        // Destroy the level at the current index
        DestroyLevelAtGivenIndex(currentLevelIndex);

        // decr the current level index
        --currentLevelIndex;

        // if we've gone negative with the indexes, wrap around so it's circular
        if(currentLevelIndex < 0)
        {
            currentLevelIndex = levelLoad.Length - 1;
        }
        SpawnLevelAtGivenIndex(currentLevelIndex);
    }

    // MENU BUTTONS

    public void DisableTitleCard()
    {
        titleObj.SetActive(false);
    }

    public void EnableTitleCard()
    {
        titleObj.SetActive(true);
    }

    public void DisableMenu()
    {
        menuObj.SetActive(false);
    }

    public void EnableMenu()
    {
        menuObj.SetActive(true);
        NextDifficulty();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
