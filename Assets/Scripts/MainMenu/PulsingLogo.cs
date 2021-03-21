using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingLogo : MonoBehaviour
{
    public float distanceToPulse;
    public float pulseSpeed;

    private float originalZ;
    private float targetZ;
    private Vector3 originalPos;
    private Vector3 targetPos;

    void Start()
    {
        originalZ = transform.position.z;
        targetZ = transform.position.z + distanceToPulse;
        originalPos = transform.position;
        targetPos = new Vector3(transform.position.x, transform.position.y, targetZ);
    }

    void Update()
    {
        float time = Mathf.PingPong(Time.time * pulseSpeed, 1);
        transform.position = Vector3.Lerp(originalPos, targetPos, time);
    }
}
