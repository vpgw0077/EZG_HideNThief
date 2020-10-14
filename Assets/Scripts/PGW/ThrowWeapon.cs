using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        FlashBang,
        Rock,
        SmokeShell
    }

    public WeaponType theThrowWeapon;
    private void Start()
    {
        InvokeRepeating("FlashBang", 0, 10f);
        Invoke("ThrowRock", 0);
        SmokeShell();
    }

    public void FlashBang()
    {
        if (theThrowWeapon == WeaponType.FlashBang)
        {

            Collider[] colls = Physics.OverlapSphere(transform.position, 10f);

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
    }

    public void ThrowRock()
    {
        if (theThrowWeapon == WeaponType.Rock)
        {

            Collider[] colls = Physics.OverlapSphere(transform.position, 20f);

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

    public void SmokeShell()
    {
        if (theThrowWeapon == WeaponType.SmokeShell)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(SmokeOff());
        }
    }

    IEnumerator SmokeOff()
    {
        yield return new WaitForSeconds(10f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

}
