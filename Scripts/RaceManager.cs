using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;
    
    public CheckPoint[] allCheckPoints;
    public int currentLap;
    public int maxLap;
    private CheckPointTracker playerTracker;

    public TextMeshProUGUI theLap;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerTracker = FindObjectOfType<CheckPointTracker>(); 
        for (int i = 0; i < allCheckPoints.Length; i++)
        {
            allCheckPoints[i].checkPointNumber = i;
        }
    }

    private void Update()
    {
        currentLap = playerTracker.laps;
        theLap.text = currentLap.ToString();
        if (currentLap == maxLap)
        {
            Debug.Log("You is winner");
        }
    }
}
