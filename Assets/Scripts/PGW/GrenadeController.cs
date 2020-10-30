using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrenadeController : MonoBehaviour
{
    public Grenade currentGrenade;

    public float ThrowForce = 20f;
    public bool ThrowReady = false;
    public bool isReady = false;
    public GameObject grenadePrefab;
    public Transform hand;



    public int HoldCount;
    public int MaxCount = 5;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrepareCoroutine());
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

        TryThrow();


    }

    protected void TryThrow()
    {
        if (HoldCount > 0)
        {

            if (Input.GetMouseButton(0) && HoldCount > 0)
            {
                ThrowReady = true;

            }

            if (Input.GetMouseButtonUp(0))
            {
                Throw();
            }

        }

    }
    private void Throw()
    {
        GameObject grenade = Instantiate(grenadePrefab, hand.position, hand.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * ThrowForce, ForceMode.VelocityChange);
        ThrowReady = false;
        --HoldCount;
    }

    public Grenade getGrenade()
    {
        return currentGrenade;
    }

    public virtual void GrenadeChange(Grenade _grenade)
    {
        if (ItemManager.currentWeapon != null)

            ItemManager.currentWeapon.gameObject.SetActive(false);




        currentGrenade = _grenade;
        ItemManager.currentWeapon = currentGrenade.GetComponent<Transform>();
        // WeaponManager_PGW.currentWeaponAnim = currentGrenade.Anim;

        currentGrenade.transform.localPosition = Vector3.zero;
        currentGrenade.gameObject.SetActive(true);

    }

}
