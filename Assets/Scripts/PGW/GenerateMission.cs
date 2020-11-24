using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMission : MonoBehaviour
{
    public bool GenerateOn = false;
    public Animator anim;

    MissionCreate theMission;

    private void Start()
    {
        theMission = FindObjectOfType<MissionCreate>();
    }
    private void Update()
    {
        if (GenerateOn)
        {
            anim.SetTrigger("Operation");
        }
    }

    public void Operation()
    {
        GenerateOn = true;
        ++theMission.CurrentGenerator;
        theMission.CheckClear();

        Collider[] colls = Physics.OverlapSphere(transform.position, 100f);

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
