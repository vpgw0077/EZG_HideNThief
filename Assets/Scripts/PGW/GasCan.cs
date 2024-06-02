using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour,IMissionTrigger
{
    private GasCanMission gasCanMission = null;

    private void Awake()
    {
        gasCanMission = FindObjectOfType<GasCanMission>();
    }

    public void TriggerMission()
    {
        gasCanMission.GetGasCan();
        gameObject.SetActive(false);
    }

}
