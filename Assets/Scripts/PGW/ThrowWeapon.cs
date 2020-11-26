using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    float Countdown = 2f;

    public ParticleSystem Smoke_Effect;
    public ParticleSystem Flash_Effect;

    public AudioClip[] OnGroundSfx;
    public AudioClip[] EmptyCanSfx;
    public AudioClip SmokeOut;
    public AudioClip FlashOut;

    public AudioSource SoundPlayer;
    public AudioSource ExplodePlayer;

    public enum WeaponType
    {
        FlashBang,
        Rock,
        SmokeShell
    }

    public WeaponType theThrowWeapon;
    private void Start()
    {
        StartCoroutine(GrenadeCooking());
    }
    IEnumerator GrenadeCooking()
    {
        yield return new WaitForSeconds(Countdown);
        GrenadeTrigger();


    }
    public void GrenadeTrigger()
    {

        if (theThrowWeapon == WeaponType.FlashBang)
        {

            ParticleSystem f_Effect = ParticlePooling.instance.GetF_Queue();
            f_Effect.transform.position = gameObject.transform.position;
            f_Effect.transform.parent = gameObject.transform;
            ExplodePlayer.clip = FlashOut;
            ExplodePlayer.Play();
            Collider[] colls = Physics.OverlapSphere(transform.position, 15f);

            foreach (var coll in colls)
            {
                var police = coll.GetComponent<EnemyAI>();
                if (police != null)
                {
                    police.isStuned = true;
                    police.StartCoroutine(police.Stun());

                }
            }
        }
        if (theThrowWeapon == WeaponType.SmokeShell)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            ParticleSystem s_Effect = ParticlePooling.instance.GetQueue();
            s_Effect.transform.parent = gameObject.transform;
            s_Effect.transform.position = gameObject.transform.position;
            ExplodePlayer.clip = SmokeOut;
            ExplodePlayer.Play();
            StartCoroutine(SmokeOff());
        }
    }

    public void ThrowRock()
    {
        if (theThrowWeapon == WeaponType.Rock)
        {

            Collider[] colls = Physics.OverlapSphere(transform.position, 200f);

            foreach (var coll in colls)
            {
                var police = coll.GetComponent<EnemyAI>();
                if (police != null)
                {
                    police.DrawAttention(transform.position);

                }
            }
        }
    }

    IEnumerator SmokeOff()
    {
        yield return new WaitForSeconds(30f);
        gameObject.transform.GetChild(0).transform.position = new Vector3(0, 200, 0);
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("Enemy"))
        {
            ThrowRock();
        }
        switch (theThrowWeapon)
        {
            case WeaponType.FlashBang: 
                SoundPlayer.clip = EmptyCanSfx[UnityEngine.Random.Range(0, OnGroundSfx.Length)];
                SoundPlayer.Play();
                break;
            case WeaponType.SmokeShell:
                SoundPlayer.clip = EmptyCanSfx[UnityEngine.Random.Range(0, OnGroundSfx.Length)];
                SoundPlayer.Play();
                break;
            case WeaponType.Rock:
                SoundPlayer.clip = OnGroundSfx[UnityEngine.Random.Range(0, OnGroundSfx.Length)];
                SoundPlayer.Play();
                break;

        }
    }

}
