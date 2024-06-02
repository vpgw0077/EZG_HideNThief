using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public delegate void Battery_EventHandler();
    public event Battery_EventHandler RechargeBattery;
    public event Battery_EventHandler LightOnEvent;

    [SerializeField] private GameObject theCamera = null;
    [SerializeField] private Light flashLight = null;
    [SerializeField] private float decreaseValue = 0.0005f;

    [SerializeField] private float battery = 0;
    private float Battery
    {
        get => battery;
        set => battery = Mathf.Clamp(value, 0, 1f);
    }

    private bool isLightON = false;

    private bool InteractActivate = false;
    private bool isClose = false;

    private RaycastHit hitInfo;

    private Outline InteractableObject = null;
    private Outline WarningOutLine = null;
    private ItemManager itemManager = null;

    private float batteryIncreaseValue = 0.2f;
    private float outLineWidth = 20f;
    private int ItemMask = 0;
    private int DetectMask = 0;
    private int trapMask = 0;

    private readonly Color32 yellowOutLine = new Color32(240, 248, 88, 255);
    private readonly Color32 blueOutLine = new Color32(0, 163, 255, 100);
    private readonly Color32 redOutLine = new Color32(255, 0, 0, 100);

    private const string tagBattery = "Battery";
    private const string tagDetectZone = "DetectZone";
    private const string tagItem = "Item";
    private const string tagTrap = "Trap";
    private const string tagMissionObject = "MissionObject";

    // Start is called before the first frame update

    private void Awake()
    {
        itemManager = GetComponentInChildren<ItemManager>();
    }
    void Start()
    {
        ItemMask = 1 << 10;
        DetectMask = 1 << 14;
        trapMask = 1 << 16;
        Battery = 1f;
    }


    // Update is called once per frame
    void Update()
    {
        TryOn();
        if (isLightON)
        {
            Battery -= decreaseValue * Time.deltaTime;
            LightOnEvent?.Invoke();
        }
        if (Battery == 0 && isLightON)
        {
            BatteryOut();
        }
        FindInteractableObject();
        FindTrap();
        CheckDistanceItem();
        TryInteract(); // 줍기 실행


    }
    private void CheckDistanceItem()
    {
        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 5f, ItemMask, QueryTriggerInteraction.Ignore))
        {
            isClose = true;
        }

        else
        {
            isClose = false;
        }
    }
    private void FindInteractableObject()
    {

        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 25f, DetectMask) && isLightON && !isClose)
        {
            if (hitInfo.transform.GetChild(0).CompareTag(tagDetectZone))
            {

                if (InteractableObject != hitInfo.transform.GetComponentInParent<Outline>()) // InteractableObject의 오브젝트와 광선의 맞은 오브젝트가 다르다면
                {                                                                            // InteractableObject의 윤곽선 제거
                    OutLineDisappear(tagDetectZone);
                }
                DrawOutLine(tagDetectZone);
            }


        }

        else if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 5f, ItemMask, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.transform.CompareTag(tagItem) || hitInfo.transform.CompareTag(tagBattery) || hitInfo.transform.CompareTag(tagMissionObject))
            {
                if (InteractableObject != hitInfo.transform.GetComponent<Outline>())
                {
                    OutLineDisappear(tagItem);
                }
                DrawOutLine(tagItem);


            }
        }
        else
        {
            OutLineDisappear(tagItem);
        }


    }

    private void FindTrap()
    {

        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, 25f, trapMask) && isLightON)
        {
            if (hitInfo.transform.CompareTag(tagTrap))
            {
                DrawOutLine(tagTrap);
            }
        }

        else
        {
            OutLineDisappear(tagTrap);
        }


    }
    private void TryOn()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Battery > 0)
            {
                LightON();

            }

        }
    }

    private void LightON()
    {
        isLightON = !isLightON;
        if (isLightON)
        {
            flashLight.enabled = true;

        }

        else
        {
            flashLight.enabled = false;
        }
    }

    private void BatteryOut()
    {
        isLightON = false;
        flashLight.enabled = false;
    }

    private void DrawOutLine(string objectTag)
    {
        switch (objectTag)
        {
            case tagDetectZone:
                if (InteractableObject == null)
                {
                    InteractableObject = hitInfo.transform.GetComponentInParent<Outline>();
                }
                InteractableObject.OutlineColor = yellowOutLine;
                InteractableObject.OutlineWidth = outLineWidth;
                break;

            case tagItem:
                if (isClose)
                {
                    InteractActivate = true;
                    if (InteractableObject == null)
                    {
                        InteractableObject = hitInfo.transform.GetComponent<Outline>();
                    }
                    InteractableObject.OutlineColor = blueOutLine;
                    InteractableObject.OutlineWidth = outLineWidth;

                }
                break;

            case tagTrap:
                if (WarningOutLine == null)
                {

                    WarningOutLine = hitInfo.transform.GetComponentInParent<Outline>();
                }
                WarningOutLine.OutlineColor = redOutLine;
                WarningOutLine.OutlineWidth = outLineWidth;
                break;
        }

    }
    private void TryInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }


    public void OutLineDisappear(string objectTag) // 윤곽선 사라짐
    {
        switch (objectTag)
        {
            case tagTrap:
                if (WarningOutLine != null)
                {
                    WarningOutLine.OutlineWidth = 0;
                    WarningOutLine = null;
                }
                break;

            default:
                if (InteractableObject != null)
                {
                    InteractActivate = false;
                    InteractableObject.OutlineWidth = 0;
                    InteractableObject = null;
                }
                break;

        }
    }

    private void GetBattery(GameObject batteryItem)
    {
        Battery += batteryIncreaseValue;
        batteryItem.SetActive(false);
    }
    private void Interact() // 줍기
    {
        if (InteractActivate)
        {

            if (hitInfo.transform.CompareTag(tagItem))
            {
                var itemInfo = hitInfo.transform.GetComponent<ItemPickUp>();
                if (itemInfo != null)
                {
                    itemManager.GetEquipment(hitInfo.transform.gameObject, itemInfo.item.itemID);

                }

            }

            else if (hitInfo.transform.CompareTag(tagBattery) && Battery < 1f)
            {
                GetBattery(hitInfo.transform.gameObject);
                RechargeBattery?.Invoke();
            }

            else
            {
                var missionInfo = hitInfo.transform.GetComponent<IMissionTrigger>();
                if (missionInfo != null)
                {
                    missionInfo.TriggerMission();
                }

            }

            SoundManager.instance.PlaySE(SfxType.itemGrab);
        }
    }


}
