using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float plateDistance = 10f;

    void Start()
    {
        // When the game begins we want the cursor to vanish
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        //Offset plate by an adjustable distance so it's far from the camera.
        mousePos.z += plateDistance;
        //Set plate to wherever the mouse is.
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
