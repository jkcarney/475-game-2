﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{
    public float plateDistance = 10f;
    public float clampX;
    public float clampYMin;
    public float clampYMax;

    public Rigidbody plateRB;

    private Vector3 mouseWorldPosition;

    void Start()
    {
        // When the game begins we want the cursor to vanish
        if(DifficultyStatic.playfabScoreboard != "NONE")
            Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
        Vector3 mousePos = Input.mousePosition;
        //Offset plate by an adjustable distance so it's far from the camera.
        //Keep plate within clampX and Y
        mousePos.z += plateDistance;
        //Get the world coordinates of the mouse position
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPosition = new Vector3(
            Mathf.Clamp(mouseWorldPosition.x, -clampX, clampX),
            Mathf.Clamp(mouseWorldPosition.y, clampYMin, clampYMax),
            mouseWorldPosition.z
        );
    }

    void FixedUpdate()
    {
        //Use movePosition because otherwise physics will not be enacted on other objects
        plateRB.MovePosition(mouseWorldPosition);
    }
}
