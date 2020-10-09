using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBangTest : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("FlashBang", 0, 10f);
    }

    public void FlashBang()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 10f);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<EnemyAI>();
            if (police != null)
            {
                police.isStuned = true;

            }
        }
    }

}
