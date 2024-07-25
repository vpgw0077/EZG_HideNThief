using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private FirstPersonMovement player = null;
    [SerializeField] private InteractObject interactObject = null;
    [SerializeField] private Equipment_EnergyDrink theDrinkController = null;
    [SerializeField] private Equipment[] theEquipment = null;

    [Header("ImageComponent")]
    [SerializeField] private Slider Stamina_Bar = null;
    [SerializeField] private Slider Battery_Bar = null;
    [SerializeField] private Image RunIcon = null;
    [SerializeField] private Image SmokeFrame = null;

    [Space]
    [Header("TextComponent")]
    [SerializeField] private Text[] itemCountText = null;

    [Space]
    [Header("Values")]
    [SerializeField] private float staminaDecreaseValue = 0.0005f;
    [SerializeField] private float batteryDecreaseValue = 0.0005f;
    [SerializeField] private float IncreaseValue = 0.005f;
    [SerializeField] private float batteryIncreaseValue = 0.2f;

    [Space]
    [Header("etc")]
    [SerializeField] private GameObject KeyGuider = null;
    [SerializeField] private GameObject OptionUI = null;

    private bool isGuideOn = false;


    private readonly Color32 chasingUIColor = new Color32(255, 40, 40, 255);
    private readonly Color32 idleUIColor = new Color32(40, 111, 255, 255);
    void Awake()
    {

        player = FindObjectOfType<FirstPersonMovement>();
        interactObject = FindObjectOfType<InteractObject>();
        interactObject.RechargeBattery += GetBattery;
        interactObject.LightOnEvent += DecreaseBattery;
        player.DecreaseStaminaEvent += DecreaseStamina;
        player.IncreaseStaminaEvent += IncreaseStamina;
        player.SmokeFrameEvent += ToggleSmokeFrame;
        theDrinkController.drinkEvent += UseEnergyDrink;
        for (int i = 0; i < theEquipment.Length; i++)
        {
            theEquipment[i].UpdateItemCountEvent += UpdateItemCount;
        }

    }
    private void Start()
    {
        GameController.instance.IdleIconEvent += ChangeIdleIconColor;
        GameController.instance.WarningIconEvent += ChangeWarningIconColor;
        GameController.instance.OptionUIEvent += ToggleOptionUI;
        Guideon();
    }
    void Update()
    {
        TryOn();

    }

    private void TryOn()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Guideon();
        }
    }

    private void Guideon()
    {
        isGuideOn = !isGuideOn;
        if (isGuideOn)
        {
            KeyGuider.SetActive(true);
        }
        else
        {
            KeyGuider.SetActive(false);
        }
    }

    private void ToggleOptionUI(bool flag)
    {
        OptionUI.SetActive(flag);
    }
    private void IncreaseStamina()
    {
        Stamina_Bar.value += IncreaseValue * Time.deltaTime;
    }

    private void DecreaseStamina()
    {
        Stamina_Bar.value -= staminaDecreaseValue * Time.deltaTime;
    }
    private void DecreaseBattery()
    {
        Battery_Bar.value -= batteryDecreaseValue * Time.deltaTime;
    }

    private void GetBattery()
    {
        Battery_Bar.value += batteryIncreaseValue;
    }

    private void ChangeWarningIconColor()
    {
        Stamina_Bar.image.color = chasingUIColor;
        RunIcon.color = chasingUIColor;

    }
    private void ChangeIdleIconColor()
    {

        Stamina_Bar.image.color = idleUIColor;
        RunIcon.color = idleUIColor;

    }

    private void UseEnergyDrink()
    {
        Stamina_Bar.value = 1f;
    }

    private void UpdateItemCount(int id, int holdCount)
    {
        itemCountText[id].text = holdCount.ToString();
    }

    private void ToggleSmokeFrame(bool isInSmoke)
    {
        SmokeFrame.gameObject.SetActive(isInSmoke);
    }


}
