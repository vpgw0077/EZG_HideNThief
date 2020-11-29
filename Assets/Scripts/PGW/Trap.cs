using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float SirenRange;
    public bool isActivate = false;
    public string SirenSfx;

    public void SirenTrigger()
    {
        isActivate = true;
        SoundManager.instance.PlaySE(SirenSfx);
        Collider[] colls = Physics.OverlapSphere(transform.position, SirenRange);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<EnemyAI>();
            if (police != null)
            {
                police.OnAware();

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isActivate)
            {
                SirenTrigger();
            }

        }
    }

}
