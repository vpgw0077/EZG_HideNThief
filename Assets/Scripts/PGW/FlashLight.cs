using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light Light;
    public bool isON = true;


    void Update()
    {
        TryOn();

    }

    public void TryOn()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LightON();
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
}
