using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [Header("KeyCode")]
    [SerializeField] private KeyCode rock_KeyCode = KeyCode.Alpha1;
    [SerializeField] private KeyCode flashBang_KeyCode = KeyCode.Alpha2;
    [SerializeField] private KeyCode smokeShell_KeyCode = KeyCode.Alpha3;
    [SerializeField] private KeyCode energyDrink_KeyCode = KeyCode.Alpha4;

    [Space]
    [HideInInspector] public Equipment currentEquipment = null;
    [HideInInspector] public Animator currentEquipAnim = null;

    [SerializeField] private Equipment[] equipments = null; // 아이템 획득을 위한 변수 
                                                            // Item의 아이디와 Hierarchy의 장비 순서와 일치해야함 Rock - flashBang - smokeShell - Drink 순


    [Space]
    [SerializeField] private Equipment equipment_Rock = null;
    [SerializeField] private Equipment equipment_flashBang = null;
    [SerializeField] private Equipment equipment_smokeShell = null;
    [SerializeField] private Equipment equipment_energyDrink = null;


    // Update is called once per frame
    void Update()
    {
        if(currentEquipment != null && currentEquipment.HoldCount == 0) // 아이템 사용 후 0개가 되면 장비를 해제
        {
            currentEquipment = null;
        }

        if (Input.GetKeyDown(rock_KeyCode))
        {

            ChangeEquipment(equipment_Rock);

        }

        else if (Input.GetKeyDown(flashBang_KeyCode))
        {

            ChangeEquipment(equipment_flashBang);

        }

        else if (Input.GetKeyDown(smokeShell_KeyCode))
        {

            ChangeEquipment(equipment_smokeShell);

        }
        else if (Input.GetKeyDown(energyDrink_KeyCode))
        {

            ChangeEquipment(equipment_energyDrink);

        }


    }
    public void GetEquipment(GameObject item, int id)
    {
        if (equipments[id].HoldCount == equipments[id].maxCount) return;

        equipments[id].HoldCount++;
        var holdCount = equipments[id].HoldCount;
        item.SetActive(false);
    }
    private void ChangeEquipment(Equipment equipment)
    {
        if (currentEquipment == equipment || equipment.HoldCount <= 0) return;

        if (currentEquipment != null)
        {
            currentEquipment.EquipOut();
        }
        currentEquipment = equipment;
        currentEquipAnim = equipment.equipAnim;
        StartCoroutine(equipment.EquipReady());

    }


}
