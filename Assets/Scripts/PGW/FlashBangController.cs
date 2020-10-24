using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBangController : GrenadeController
{
    public static bool isActivate = false;

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryThrow();
        else
        {
            isReady = false;
        }
    }
    public override void GrenadeChange(Grenade _grenade)
    {
        base.GrenadeChange(_grenade);
        isActivate = true;
    }
}
