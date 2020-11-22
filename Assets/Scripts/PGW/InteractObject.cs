using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject theCamera;
    public bool InteractActivate;
    public bool isClose;

    public RaycastHit hitInfo;

    public Outline InteractableObject;
    public MissionCreate theMission;

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
    }


    // Update is called once per frame
    void Update()
    {
        TryInteract();
        InteractableOutLine();
        CheckItem();

    }

    private void InteractableOutLine()
    {
        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 2f))
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

    private void InteractOutLine()
    {
        InteractActivate = true;
        InteractableObject = hitInfo.transform.GetComponentInParent<Outline>();
        InteractableObject.OutlineColor = new Color32(0, 163, 255, 100);
        InteractableObject.OutlineWidth = 20f;

    }

    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    private void CheckItem()
    {
        if (theLight.isON)
        {
            if (!isClose)
            {
                if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 10f))
                {
                    if (hitInfo.transform.GetChild(0).CompareTag("DetectZone"))
                    {
                        if (InteractableObject != hitInfo.transform.GetComponent<Outline>())
                        {
                            OutLineDisappear();
                        }
                        DrawOutLine();

                    }
                    else if (hitInfo.transform.CompareTag("Trap"))
                    {
                        TrapOutLine();
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


    }
    private void TrapOutLine()
    {
        InteractableObject = hitInfo.transform.GetComponentInParent<Outline>();
        InteractableObject.OutlineColor = new Color32(255, 0, 0, 100);
        InteractableObject.OutlineWidth = 20f;
    }
    public void DrawOutLine()
    {
        InteractableObject = hitInfo.transform.GetComponentInParent<Outline>();
        InteractableObject.OutlineColor = new Color32(255, 255, 255, 100);
        InteractableObject.OutlineWidth = 20f;

    }

    public void OutLineDisappear()
    {
        if (InteractableObject != null)
        {
            InteractableObject.OutlineWidth = 0;
            InteractActivate = false;
        }
    }
    private void Interact()
    {
        if (InteractActivate)
        {
            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Battery && theLight.Battery_Bar.value < 1)
            {
                theLight.Battery_Bar.value += 0.2f;
                hitInfo.transform.gameObject.SetActive(false);
                OutLineDisappear();
            }


            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Generator)
            {
                var Gen = hitInfo.transform.GetComponent<GenerateMission>();
                if (!Gen.GenerateOn)
                {
                    Gen.Operation();
                    OutLineDisappear();
                }
            }

            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.GasCan)
            {
                ++theMission.CurrentGascan;
                Destroy(hitInfo.transform.gameObject);
                OutLineDisappear();

            }
            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.Rock && !(theRock.HoldCount >= theRock.MaxCount))
            {
                theRock.HoldCount += 1;
                theRock.UpdateCount();
                hitInfo.transform.gameObject.SetActive(false);
                OutLineDisappear();


            }
            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.FlashBang && !(theFlash.HoldCount >= theFlash.MaxCount))
            {
                theFlash.HoldCount += 1;
                theFlash.UpdateCount();
                hitInfo.transform.gameObject.SetActive(false);
                OutLineDisappear();

            }
            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.SmokeShell && !(theSmoke.HoldCount >= theSmoke.MaxCount))
            {
                theSmoke.HoldCount += 1;
                theSmoke.UpdateCount();
                hitInfo.transform.gameObject.SetActive(false);
                OutLineDisappear();

            }
            if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemType == Item.ItemName.EnergyDrink && !(theDrink.HoldCount >= theDrink.MaxCount))
            {
                theDrink.HoldCount += 1;
                theDrink.UpdateCount();
                hitInfo.transform.gameObject.SetActive(false);
                OutLineDisappear();


            }


        }
    }


}
