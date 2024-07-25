using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Grenade : Equipment
{
    [SerializeField] private Transform throwPos = null;
    [SerializeField] private GameObject grenadePrefab = null;
    [SerializeField] private float throwForce = 30f;
    [SerializeField] private bool throwReady = false;

    public override void EquipOut()
    {
        StopCoroutine(EquipReady());
        throwReady = false;
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
                throwReady = true;
                equipAnim.SetTrigger("ThrowReady");

            }

            if (Input.GetMouseButtonUp(0) && throwReady && currentFireRate <= 0)
            {
                equipAnim.SetTrigger("Throw");
                StartCoroutine(UseEquipment());
            }

        }
    }

    protected override IEnumerator UseEquipment()
    {
        currentFireRate = 1f;
        yield return usingDelay;
        GameObject grenade = Instantiate(grenadePrefab, throwPos.position, throwPos.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(throwPos.forward * throwForce, ForceMode.Impulse);
        SoundManager.instance.PlaySE(SfxType.grenadeThrow);
        throwReady = false;
        --HoldCount;
    }


}
