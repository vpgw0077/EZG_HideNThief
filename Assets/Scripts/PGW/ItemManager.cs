using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public static bool isChangeWeapon = false;
    public string currentWeaponType; // 현재 무기 타입 이름
    public static Transform currentWeapon;  // 현재 무기
    public static Animator currentWeaponAnim;
    public static int test = 4;

    public float changeWeaponDelayTime; // 바꾸기 딜레이
    public float changeWeaponEndDelayTime; // 바꾼 후 딜레이




    public Grenade[] grenades; // 투척무기 리스트
    public Energy_Drink[] drinks;
    public Hand[] Hands;



    public Dictionary<string, Grenade> grenadeDictionary = new Dictionary<string, Grenade>();
    public Dictionary<string, Energy_Drink> drinkDictionary = new Dictionary<string, Energy_Drink>();
    public Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();




    public RockController theRockController;
    public FlashBangController theFlashController;
    public SmokeShellController theSmokeController;
    public DrinkController theDrinkController;
    public HandController theHandController;



    private void Awake()
    {
        isChangeWeapon = false;
    }
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < grenades.Length; i++)
        {
            grenadeDictionary.Add(grenades[i].grenadName, grenades[i]);
        }
        for (int i = 0; i < drinks.Length; i++)
        {
            drinkDictionary.Add(drinks[i].DrinkName, drinks[i]);
        }
        for (int i = 0; i < Hands.Length; i++)
        {
            handDictionary.Add(Hands[i].HandName, Hands[i]);
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && RockController.isActivate == false && theRockController.HoldCount > 0)
            {
                StartCoroutine(ChangeWeaponCoroutine("GRENADE_ROCK", "Rock"));

            }


            else if (Input.GetKeyDown(KeyCode.Alpha2) && FlashBangController.isActivate == false && theFlashController.HoldCount > 0)
            {

                StartCoroutine(ChangeWeaponCoroutine("GRENADE_FLASH", "FlashBang"));

            }

            else if (Input.GetKeyDown(KeyCode.Alpha3) && SmokeShellController.isActivate == false && theSmokeController.HoldCount > 0)
            {

                StartCoroutine(ChangeWeaponCoroutine("GRENADE_SMOKE", "SmokeShell"));

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && DrinkController.isActivate == false && theDrinkController.HoldCount > 0)
            {

                StartCoroutine(ChangeWeaponCoroutine("ENERGY_DRINK", "EnergyDrink"));

            }
        }

    }
    public void RunoutItem()
    {

        StartCoroutine(ChangeWeaponCoroutine("HAND", "EmptyHand"));


    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);


        CanclePreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    public void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GRENADE_ROCK":
                RockController.isActivate = false;
                break;
            case "GRENADE_FLASH":
                FlashBangController.isActivate = false;
                break;
            case "GRENADE_SMOKE":
                SmokeShellController.isActivate = false;
                break;
            case "ENERGY_DRINK":
                DrinkController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;

        }
    }
    public void WeaponChange(string _type, string _name)
    {


        if (_type == "GRENADE_ROCK")
        {
            theRockController.GrenadeChange(grenadeDictionary[_name]);

            theRockController.StartCoroutine(theRockController.PrepareCoroutine());

        }
        if (_type == "GRENADE_FLASH")
        {

            theFlashController.GrenadeChange(grenadeDictionary[_name]);



            theFlashController.StartCoroutine(theFlashController.PrepareCoroutine());

        }
        if (_type == "GRENADE_SMOKE")
        {

            theSmokeController.GrenadeChange(grenadeDictionary[_name]);


            theSmokeController.StartCoroutine(theSmokeController.PrepareCoroutine());
        }
        if (_type == "ENERGY_DRINK")
        {

            theDrinkController.DrinkChange(drinkDictionary[_name]);


            theDrinkController.StartCoroutine(theDrinkController.PrepareCoroutine());
        }
        if (_type == "HAND")
        {
            theHandController.HandChange(handDictionary[_name]);
            theHandController.StartCoroutine(theHandController.PrepareCoroutine());
        }





    }

}
