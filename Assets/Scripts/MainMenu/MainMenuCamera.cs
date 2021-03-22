using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuCamera : MonoBehaviour
{
    // Our Level prefabs are stored in this array
    public GameObject[] levelLoad;

    public GameObject titleObj;
    public GameObject menuObj;
    public GameObject promptObj;

    public float rotationSpeed;

    public float gravityPower;

    private Vector3 target_rotation;
    private bool isFacingSky = true;
    
    // Start current level index at -1 for the main menu
    private int currentLevelIndex = -1;

    private string path;

    void Start()
    {
        path = Application.dataPath + "/PleaseDontModifyThisPleaseDontMakeMeWriteAnEncryptionAlgorithm.txt";
        Physics.gravity = new Vector3(0.0f, gravityPower, 0.0f);
    }

    void Update()
    {
        // Rotate towards the desired rotation which gets updated in the Rotate function
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target_rotation, Time.deltaTime * rotationSpeed);
    }

    public void MainMenuValidateUsernameInitialized()
    {
        // If a username is already initialized, it's buisness as usual
        if(File.Exists(path))
        {
            DisableTitleCard();
            EnableMenu();
            Rotate180Degrees();
        }
        // No username file exists; prompt the user.
        else
        {
            DisableTitleCard();
            EnableUsernamePrompt();
        }
    }

    public void WriteUsernameToFile(string username)
    {
        if(!File.Exists(path))
        {
            File.WriteAllText(path, username);
        }
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
        GameObject info = Instantiate(levelLoad[index], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        // Grab the LevelInformation from the instaniated object
        LevelInformation level = info.GetComponent<LevelInformation>();

        // Update the main menu and static class
        UpdateMainMenuUI(level);
        UpdateStaticClass(level);

        // Have scoreboard display information for specified level
        DisplayPlayfabScoreboard(index);
    }

    // Updates the main menu components with the level information
    public void UpdateMainMenuUI(LevelInformation level)
    {
        // Use transform.Find to get the text from the menu UI
        Text titleText = menuObj.transform.Find("LevelInfo/TitlePanel/TitleText").gameObject.GetComponent<Text>();
        Text levelDescriptionText = menuObj.transform.Find("LevelInfo/LevelDescription/LevelDescriptionText").gameObject.GetComponent<Text>();

        // Update text components appropriately
        titleText.text = level.levelTitle;
        titleText.fontSize = level.titleFontSize;
        levelDescriptionText.text = level.levelDescription;
    }

    public void UpdateStaticClass(LevelInformation level)
    {
        // Static class gets updated with the level information
        DifficultyStatic.difficulty = level.difficulty;
        DifficultyStatic.trashChance = level.percentChanceForGarbage;
        DifficultyStatic.fallingSpeed = level.fallingSpeed;

        // Log for sanity check
        Debug.Log("DIFFICULTY: " + DifficultyStatic.difficulty + " TRASH CHANCE: " + DifficultyStatic.trashChance);
    }

    public void DisplayPlayfabScoreboard(int index)
    {
        return;
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

    public void StartGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
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

    public void DisableUsernamePrompt()
    {
        promptObj.SetActive(false);
    }

    public void EnableUsernamePrompt()
    {
        promptObj.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleVSync(bool on)
    {
        if(on)
        {
            QualitySettings.vSyncCount = 1;
            Debug.Log("VSync On");
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Debug.Log("VSync Off");
        }
    }
}
