using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointChecker : MonoBehaviour
{
    public CheckPointTracker cpTracker;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            cpTracker.CheckPointHit(other.GetComponent<CheckPoint>().checkPointNumber);
        }
    }
}
