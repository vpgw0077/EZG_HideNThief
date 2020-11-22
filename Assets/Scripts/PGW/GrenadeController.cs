using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GrenadeController : MonoBehaviour
{
    [SerializeField]
    protected Grenade currentGrenade;

    public string ThrowSound;

    protected float ThrowForce = 20f;
    protected bool ThrowReady = false;
    protected bool isReady = false;
    public GameObject grenadePrefab;
    public Transform hand;
    public Text HoldText;

    public float currentFireRate;


    public int HoldCount;
    public int MaxCount = 5;

    ItemManager itemManager;


    // Start is called before the first frame update
    private void Awake()
    {
        HoldCount = 5;
        UpdateCount();
        itemManager = GetComponent<ItemManager>();
    }
    void Start()
    {
        StartCoroutine(PrepareCoroutine());
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
    public void FireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    public void UpdateCount()
    {
        HoldText.text = HoldCount.ToString();
        if(HoldCount == 0)
        {
            itemManager.RunoutItem();

        }
    }
    public void TryThrow()
    {
        if (HoldCount > 0)
        {

            if (Input.GetMouseButtonDown(0) && HoldCount > 0 && currentFireRate <= 0)
            {
                ThrowReady = true;
                currentGrenade.anim.SetTrigger("ThrowReady");

            }

            if (Input.GetMouseButtonUp(0) && ThrowReady && currentFireRate <= 0)
            {
                currentGrenade.anim.SetTrigger("Throw");            
                StartCoroutine(Throw());
            }

        }

    }


    public IEnumerator Throw()
    {
        currentFireRate = 1f;
        yield return new WaitForSeconds(0.35f);
        GameObject grenade = Instantiate(grenadePrefab, hand.position, hand.rotation);
        SoundManager.instance.PlaySE(ThrowSound);
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * ThrowForce, ForceMode.Impulse);
        ThrowReady = false;
        --HoldCount;
        UpdateCount();
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
        ItemManager.currentWeaponAnim = currentGrenade.anim;

        currentGrenade.transform.localPosition = Vector3.zero;
        currentGrenade.gameObject.SetActive(true);

    }

}
