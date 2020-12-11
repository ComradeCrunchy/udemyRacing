using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CarController target;
    private Vector3 offsetDir;
    public float minDistance = 20f, maxDistance = 50f;
    private float activeDistance;

    private void Start()
    {
        activeDistance = minDistance;
        offsetDir = transform.position - target.transform.position;
        offsetDir.Normalize();
    }

    private void Update()
    {
        activeDistance = minDistance + ((maxDistance - minDistance) * (target.rb.velocity.magnitude / target.maxSpeed));
        transform.position = target.transform.position + (offsetDir * activeDistance);
    }
}
