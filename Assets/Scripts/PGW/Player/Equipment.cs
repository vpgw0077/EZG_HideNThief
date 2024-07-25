using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Rock,
    FlashBang,
    SmokeShell,
    Drink
}
public abstract class Equipment : MonoBehaviour
{
    public delegate void ItemUse_EventHandler(int id, int currentCount);
    public ItemUse_EventHandler UpdateItemCountEvent;

    [SerializeField] protected EquipmentType equipType = EquipmentType.Rock;

    public Animator equipAnim = null;

    public EquipmentType EquipType
    {
        get => equipType;
        private set => equipType = value;
    }

    [SerializeField] protected float delayTime = 0.6f;
    [SerializeField] protected float equipReadyTime = 0.5f;
    [SerializeField] protected GameObject equipPrefab = null;
    [SerializeField] protected bool isActivate = false;

    [SerializeField] protected int holdCount = 0;
    public int HoldCount
    {
        get => holdCount;
        set
        {
            holdCount = value;
            UpdateItemCountEvent?.Invoke((int)equipType, HoldCount);

        }
    }
    public readonly int maxCount = 5;

    [SerializeField] protected float currentFireRate = 0;
    protected WaitForSeconds usingDelay;
    protected WaitForSeconds equipReadyDelay;


    private void Start()
    {
        usingDelay = new WaitForSeconds(delayTime); // 장비를 사용하기 까지의 딜레이
        equipReadyDelay = new WaitForSeconds(equipReadyTime); // 장비를 바꾸고 활성화가 되기까지의 딜레이
        equipAnim = GetComponent<Animator>();
        HoldCount = 3;
        gameObject.SetActive(false);
    }

    public IEnumerator EquipReady()
    {
        if (isActivate) isActivate = false;
        equipPrefab.transform.localPosition = Vector3.zero;
        equipPrefab.transform.localRotation = Quaternion.identity;
        equipPrefab.SetActive(true);
        yield return equipReadyDelay;
        isActivate = true;
    }

    protected void Update()
    {
        if (isActivate)
        {
            TryUseEquipment();
            FireRateCalc();
        }

        if (holdCount == 0)
        {
            EquipOut();
        }
    }
    protected virtual void FireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    public abstract void EquipOut();
    protected abstract void TryUseEquipment();

    protected abstract IEnumerator UseEquipment();

}
