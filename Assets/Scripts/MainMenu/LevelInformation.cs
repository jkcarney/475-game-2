using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
    public int difficulty;
    public float percentChanceForGarbage;
    public string levelTitle;
    public int titleFontSize = 150;
    public float fallingSpeed = -9.81f;
    [TextArea(15,20)]
    public string levelDescription;
    
}
