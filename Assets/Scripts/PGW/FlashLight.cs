using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    public Light Light;
    public bool isON = true;
    public Slider Battery_Bar;

    public GameObject KeyGuider;
    public bool isGuideOn = true;

    private void Start()
    {
        isGuideOn = true;
    }
    void Update()
    {
        TryOn();
        if (isON)
        {
            Battery_Bar.value -= 0.00005f;
        }
        if (Battery_Bar.value == 0)
        {
            BatteryOut();
        }

    }

    public void TryOn()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Battery_Bar.value > 0)
            {
                LightON();

            }

        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Guideon();
        }
    }

    private void Guideon()
    {
        isGuideOn = !isGuideOn;
        if (isGuideOn)
        {
            KeyGuider.SetActive(true);
        }
        else
        {
            KeyGuider.SetActive(false);
        }
    }

    public void LightON()
    {
        isON = !isON;
        if (isON)
        {
            Light.enabled = true;
            
        }

        else
        {
            Light.enabled = false;
        }
    }

    public void BatteryOut()
    {
        isON = false;
        Light.enabled = false;
    }
}
