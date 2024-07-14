using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_GasCan : Generator,IMissionTrigger
{
    private GasCanMission gasCanMission = null;

    private void Awake()
    {
        gasCanMission = FindObjectOfType<GasCanMission>();
    }
    public void TriggerMission()
    {
        if (!isActivate && gasCanMission.IsGetAllGasCan)
        {
            OperateGenerator();
            gasCanMission.MissionClear();
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
