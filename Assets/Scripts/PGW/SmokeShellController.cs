using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShellController : GrenadeController
{
    public static bool isActivate = false;

    // Update is called once per frame
    private void Awake()
    {
        isActivate = false;

    }
    void Update()
    {
        if (isActivate && !ItemManager.isChangeWeapon)
            TryThrow();
        else
        {
            isReady = false;
        }

        if (isActivate)
        {
           
            FireRateCalc();
        }
    }

    public override void GrenadeChange(Grenade _grenade)
    {
        base.GrenadeChange(_grenade);
        isActivate = true;
    }
}
