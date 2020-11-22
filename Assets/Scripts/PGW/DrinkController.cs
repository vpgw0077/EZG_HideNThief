using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkController : MonoBehaviour
{
    public Energy_Drink theDrink;

    public static bool isActivate = false;
    public bool isReady = false;

    public float currentFireRate;

    public int HoldCount;
    public int MaxCount = 5;

    public Text HoldText;

    FirstPersonMovement thePlayer;
    ItemManager itemManager;


    private void Awake()
    {
        HoldCount = 5;
        UpdateCount();
        itemManager = GetComponent<ItemManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrepareCoroutine());
        thePlayer = GetComponentInParent<FirstPersonMovement>();
    }

    public void FireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    public void UpdateCount()
    {
        HoldText.text = HoldCount.ToString();
        if (HoldCount == 0)
        {
            itemManager.RunoutItem();

        }
    }

    public IEnumerator PrepareCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isReady = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActivate && !ItemManager.isChangeWeapon)
        {
            TryDrink();

        }
        if (isActivate)
        {
            FireRateCalc();
        }
    }

    private void TryDrink()
    {
        if (HoldCount > 0)
        {

            if (Input.GetMouseButtonDown(0) && HoldCount > 0 && !thePlayer.useDrink && currentFireRate <= 0)
            {
                theDrink.anim.SetTrigger("Drink");
                StartCoroutine(Drink());

            }

        }
    }

    IEnumerator Drink()
    {
        currentFireRate = 1f;
        yield return new WaitForSeconds(0.5f);
        thePlayer.useDrink = true;
        --HoldCount;
        UpdateCount();
        StartCoroutine(DrinkOver());
    }

    public IEnumerator DrinkOver()
    {
        yield return new WaitForSeconds(10f);
        thePlayer.useDrink = false;
    }

    public Energy_Drink getDrink()
    {
        return theDrink;
    }

    public void DrinkChange(Energy_Drink _drink)
    {
        if (ItemManager.currentWeapon != null)

            ItemManager.currentWeapon.gameObject.SetActive(false);




        theDrink = _drink;
        ItemManager.currentWeapon = theDrink.GetComponent<Transform>();
        ItemManager.currentWeaponAnim = theDrink.anim;

        theDrink.transform.localPosition = Vector3.zero;
        theDrink.gameObject.SetActive(true);
        isActivate = true;

    }
}
