using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMission : MonoBehaviour
{
    public bool GenerateOn = false;
    MissionCreate theMission;

    private void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
    }

    public void Operation()
    {
        GenerateOn = true;
        ++theMission.CurrentGenerator;
        theMission.CheckClear();

        Collider[] colls = Physics.OverlapSphere(transform.position, 40f);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<EnemyAI>();
            if (police != null)
            {
                police.OnAware();

            }
        }
    }

}
