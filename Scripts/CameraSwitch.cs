using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCam++;
            if (currentCam >= cameras.Length)
            {
                currentCam = 0;
            }

            for (int i = 0; i < cameras.Length; i++)
            {
                if (i == currentCam)
                {
                    cameras[i].SetActive(true);
                }
                else
                {
                    cameras[i].SetActive(false);
                }
            }
        }
    }
}
