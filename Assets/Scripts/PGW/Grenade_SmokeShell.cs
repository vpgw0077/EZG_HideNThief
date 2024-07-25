using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_SmokeShell : Grenade, IGrenadeTrigger
{
    [SerializeField] private GameObject smokeEffectObj = null;
    [SerializeField] private AudioClip smokeOutClip = null;
    [SerializeField] private AudioSource greandeAudioPlayer = null;

    private WaitForSeconds smokeDuration = new WaitForSeconds(30f);
    private WaitForSeconds setAcitiveFalseTime = new WaitForSeconds(0.1f);

    private Vector3 smokePosition = new Vector3(0, 200f, 0);


    void Start()
    {
        smokeEffectObj = gameObject.transform.GetChild(0).gameObject;
        StartCoroutine(IGrenadeCooking());
    }

    public IEnumerator IGrenadeCooking()
    {
        yield return new WaitForSeconds(countDown);
        GrenadeTrigger();

    }

    protected override void GrenadeTrigger()
    {

        smokeEffectObj.SetActive(true);
        ParticleSystem s_Effect = ParticlePooling.instance.GetQueue();
        s_Effect.transform.parent = gameObject.transform;
        s_Effect.transform.position = gameObject.transform.position;
        greandeAudioPlayer.clip = smokeOutClip;
        greandeAudioPlayer.Play();
        StartCoroutine(SmokeOff());

    }

   private IEnumerator SmokeOff()
    {
        yield return smokeDuration;
        smokeEffectObj.transform.position = smokePosition;
        yield return setAcitiveFalseTime;
        smokeEffectObj.SetActive(false);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        onGroundAudioPlayer.clip = onGroundSfx[Random.Range(0, onGroundSfx.Length)];
        onGroundAudioPlayer.Play();
    }
}
