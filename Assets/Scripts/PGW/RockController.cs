using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : GrenadeController
{
    public static bool isActivate = false;

    private void Start()
    {
        ItemManager.currentWeapon = currentGrenade.GetComponent<Transform>();
        ItemManager.currentWeaponAnim = currentGrenade.anim;
    }

    // Update is called once per frame
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
