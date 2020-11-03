using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    public Light Light;
    public bool isON = true;
    public Slider Battery_Bar;


    void Update()
    {
        TryOn();
        if (isON)
        {
            Battery_Bar.value -= 0.0005f;
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
