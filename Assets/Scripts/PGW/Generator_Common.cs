using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_Common : Generator,IMissionTrigger
{
    private GenerateMission genMission = null;

    private void Awake()
    {
        genMission = FindObjectOfType<GenerateMission>();
    }

    public void TriggerMission()
    {
        if (!isActivate)
        {
            OperateGenerator();
            genMission.UpdateMission();
        }

    }

    protected override void OperateGenerator()
    {
        isActivate = true;
        audioSource.Play();
        generatorAnim.SetTrigger("Operation");
        generatorLight.SetActive(true);

        Collider[] colls = Physics.OverlapSphere(transform.position, alarmRange);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<IAlarmRespond>();
            if (police != null)
            {
                police.RespondToAlarm();
            }
        }
    }
}
