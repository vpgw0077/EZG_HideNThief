using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkController : MonoBehaviour
{
    public Energy_Drink theDrink;

    public static bool isActivate = false;
    public bool isReady = false;

    public int HoldCount;
    public int MaxCount = 5;

    FirstPersonMovement thePlayer;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrepareCoroutine());
        thePlayer = GetComponentInParent<FirstPersonMovement>();
        HoldCount = 5;
    }

    public IEnumerator PrepareCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isReady = true;
    }
    // Update is called once per frame
    void Update()
    {
        TryDrink();
    }

    private void TryDrink()
    {
        if (HoldCount > 0)
        {

            if (Input.GetMouseButtonDown(0) && HoldCount > 0)
            {
                StartCoroutine(Drink());

            }

        }
    }

    IEnumerator Drink()
    {
        yield return new WaitForSeconds(0.5f);
        thePlayer.useDrink = true;
    }

    public Energy_Drink getDrink()
    {
        return theDrink;
    }

    public virtual void DrinkChange(Energy_Drink _drink)
    {
        if (ItemManager.currentWeapon != null)

            ItemManager.currentWeapon.gameObject.SetActive(false);




        theDrink = _drink;
        ItemManager.currentWeapon = theDrink.GetComponent<Transform>();
        // WeaponManager_PGW.currentWeaponAnim = currentGrenade.Anim;

        theDrink.transform.localPosition = Vector3.zero;
        theDrink.gameObject.SetActive(true);

    }
}
