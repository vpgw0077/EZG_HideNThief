using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_EnergyDrink : Equipment
{
    public delegate void DringEventHandler();
    public event DringEventHandler drinkEvent;

    public override void EquipOut()
    {
        StopCoroutine(EquipReady());
        isActivate = false;
        currentFireRate = 0;
        equipPrefab.SetActive(false);
    }
    protected override void TryUseEquipment()
    {
        if (HoldCount > 0)
        {

            if (Input.GetMouseButtonDown(0) && currentFireRate <= 0)
            {
                equipAnim.SetTrigger("Drink");
                StartCoroutine(UseEquipment());

            }

        }
    }

    protected override IEnumerator UseEquipment()
    {
        currentFireRate = 1f;
        yield return usingDelay;
        --HoldCount;
        drinkEvent?.Invoke();
    }

}
