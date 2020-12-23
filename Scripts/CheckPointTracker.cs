using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTracker : MonoBehaviour
{
    private CarController controller;
    private int nextCP;
    public int laps;
    

    private void Start()
    {
        controller = GetComponent<CarController>();
    }

    public void CheckPointHit(int cpNumber)
    {
        Debug.Log("Hit checkpoint" + cpNumber);
        if (cpNumber == nextCP)
        {
            nextCP++;
            if (nextCP == RaceManager.instance.allCheckPoints.Length)
            {
                nextCP = 0;
                laps++;
                Debug.Log(laps);
            }
        }
    }
}
