using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    float Countdown = 2f;
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

            Collider[] colls = Physics.OverlapSphere(transform.position, 20f);

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
            StartCoroutine(SmokeOff());
        }
    }

    public void ThrowRock()
    {
        if (theThrowWeapon == WeaponType.Rock)
        {

            Collider[] colls = Physics.OverlapSphere(transform.position, 100f);

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
        yield return new WaitForSeconds(10f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("Enemy"))
        {
            ThrowRock();
        }
    }

}
