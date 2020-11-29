using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject theCamera;
    public bool InteractActivate;
    public bool isClose;

    RaycastHit hitInfo;
    RaycastHit Trapinfo;

    public Outline InteractableObject;
    public Outline WarningOutLine;
    public MissionCreate theMission;

    public int ItemMask;
    public int DetectMask;
    public string GrabSound;

    RockController theRock;
    SmokeShellController theSmoke;
    FlashBangController theFlash;
    DrinkController theDrink;
    FlashLight theLight;
    // Start is called before the first frame update

    void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
        theLight = GetComponent<FlashLight>();
        theRock = GetComponentInChildren<RockController>();
        theSmoke = GetComponentInChildren<SmokeShellController>();
        theFlash = GetComponentInChildren<FlashBangController>();
        theDrink = GetComponentInChildren<DrinkController>();
        ItemMask = 1 << 10;
        DetectMask = 1 << 14;

    }


    // Update is called once per frame
    void Update()
    {
        InteractableOutLine(); // 아이템 확인
        CheckTrap(); //함정 확인
        CheckItem();
        TryInteract(); // 줍기 실행


    }


    private void InteractableOutLine()
    {
        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 5f, ItemMask))
        {
            isClose = true;
            if (hitInfo.transform.CompareTag("Item"))
            {
                if (InteractableObject != hitInfo.transform.GetComponent<Outline>())
                {
                    OutLineDisappear();
                }
                InteractOutLine();


            }
            else
            {
                OutLineDisappear();
            }
        }
        else
        {
            isClose = false;
            OutLineDisappear();
        }

    }
    private void CheckItem()
    {
        if (theLight.isON && !isClose)
        {
            if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 25f, DetectMask))
            {
                if (hitInfo.transform.GetChild(0).CompareTag("DetectZone"))
                {

                    if (InteractableObject != hitInfo.transform.GetComponentInParent<Outline>())
                    {
                        OutLineDisappear();
                    }
                    DrawOutLine();
                }
                else
                {
                    OutLineDisappear();
                }
            }
            else
            {
                OutLineDisappear();
            }
        }

    }
    public void DrawOutLine()
    {
        InteractableObject = hitInfo.transform.GetComponentInParent<Outline>();
        InteractableObject.OutlineColor = new Color32(240, 248, 88, 255);
        InteractableObject.OutlineWidth = 20f;

    }

    private void InteractOutLine() // 윤곽선 그리기
    {
        if (isClose)
        {
            InteractActivate = true;
            InteractableObject = hitInfo.transform.GetComponent<Outline>();
            InteractableObject.OutlineColor = new Color32(0, 163, 255, 100);
            InteractableObject.OutlineWidth = 20f;


        }
    }

    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    private void CheckTrap()
    {
        if (theLight.isON)
        {
            if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out Trapinfo, 12f))
            {
                if (Trapinfo.transform.CompareTag("Trap"))
                {
                    TrapOutLine();
                }
            }
            else
            {
                TrapOutlineDisappear();
            }
        }
        else
        {
            TrapOutlineDisappear();
        }
    }
    private void TrapOutLine()
    {
        WarningOutLine = Trapinfo.transform.GetComponentInParent<Outline>();
        WarningOutLine.OutlineColor = new Color32(255, 0, 0, 100);
        WarningOutLine.OutlineWidth = 20f;
    }


    public void OutLineDisappear() // 윤곽선 사라짐
    {
        if (InteractableObject != null)
        {
            InteractActivate = false;
            InteractableObject.OutlineWidth = 0;
        }
    }
    public void TrapOutlineDisappear()
    {
        if (WarningOutLine != null)
        {
            WarningOutLine.OutlineWidth = 0;
        }
    }
    private void Interact() // 줍기
    {
        if (InteractActivate)
        {
            if (hitInfo.transform.GetComponent<ItemPickUp>() != null)
            {
                if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Generator)
                {
                    var Gen = hitInfo.transform.GetComponent<GenerateMission>();
                    if (!Gen.GenerateOn)
                    {
                        Gen.Operation();
                        OutLineDisappear();
                    }
                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Battery)
                {
                    if (theLight.Battery_Bar.value < 1)
                    {
                        theLight.Battery_Bar.value += 0.2f;
                        OutLineDisappear();
                        SoundManager.instance.PlaySE(GrabSound);
                        hitInfo.transform.gameObject.SetActive(false);

                    }
                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.GasCanGenerator)
                {
                    var Gen = hitInfo.transform.GetComponent<GenerateMission>();
                    Gen.Operation();
                    OutLineDisappear();

                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.GasCan)
                {
                    ++theMission.CurrentGascan;
                    OutLineDisappear();
                    SoundManager.instance.PlaySE(GrabSound);
                    hitInfo.transform.gameObject.SetActive(false); ;

                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Rock && !(theRock.HoldCount >= theRock.MaxCount))
                {
                    theRock.HoldCount += 1;
                    theRock.UpdateCount();
                    OutLineDisappear();
                    SoundManager.instance.PlaySE(GrabSound);
                    hitInfo.transform.gameObject.SetActive(false);


                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.FlashBang && !(theFlash.HoldCount >= theFlash.MaxCount))
                {
                    theFlash.HoldCount += 1;
                    theFlash.UpdateCount();
                    OutLineDisappear();
                    SoundManager.instance.PlaySE(GrabSound);
                    hitInfo.transform.gameObject.SetActive(false);

                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.SmokeShell && !(theSmoke.HoldCount >= theSmoke.MaxCount))
                {
                    theSmoke.HoldCount += 1;
                    theSmoke.UpdateCount();
                    OutLineDisappear();
                    SoundManager.instance.PlaySE(GrabSound);
                    hitInfo.transform.gameObject.SetActive(false);

                }
                else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.EnergyDrink && !(theDrink.HoldCount >= theDrink.MaxCount))
                {
                    theDrink.HoldCount += 1;
                    theDrink.UpdateCount();
                    OutLineDisappear();
                    SoundManager.instance.PlaySE(GrabSound);
                    hitInfo.transform.gameObject.SetActive(false);


                }
            }

        }
    }


}
