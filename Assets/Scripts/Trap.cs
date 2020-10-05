using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType
    {
        DamageTrap,
        SirenTrap
    }

    public TrapType theTrapType;
    public float SirenRange;

    public void SirenTrigger()
    {
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
            switch (theTrapType)
            {
                case TrapType.DamageTrap:
                    Debug.Log("체력 감소");
                    break;

                case TrapType.SirenTrap:
                    SirenTrigger();
                    break;

            }
        }
    }

}
