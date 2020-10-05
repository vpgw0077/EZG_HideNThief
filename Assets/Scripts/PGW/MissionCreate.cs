using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MissionCreate : MonoBehaviour
{
    public enum MissionList
    {
        Generator,
        Gascan,
        Survive
    }
    public GameObject Generator;
    public int CurrentGenerator = 0;
    public int RequireGenerator = 3;

    public GameObject GasCan;
    public int CurrentGascan = 0;
    public int RequireGascan = 5;

    public MissionList theMission;

    MissionManager theManager;



    private void Start()
    {
        theManager = FindObjectOfType<MissionManager>();
        StartMission();
    }

    private void StartMission()
    {
        switch (theMission)
        {
            case MissionList.Generator:
                CreateGenerator();
                break;

            case MissionList.Gascan:
                CreateGasCan();
                break;

            case MissionList.Survive:
                SurviveMission();
                break;
        }
    }

    private void CreateGenerator()
    {
        GameObject[] Gen = GameObject.FindGameObjectsWithTag("SpawnZone");
        List<GameObject> ob = Gen.OfType<GameObject>().ToList();

        for (int i = 0; i < 3; i++)
        {

            var SpawnZone = UnityEngine.Random.Range(0, ob.Count);
            Vector3 tr = ob[SpawnZone].transform.position;
            Instantiate(Generator, tr, Quaternion.identity);
            ob.RemoveAt(SpawnZone);


        }
    }
    private void CreateGasCan()
    {
        GameObject[] Gen = GameObject.FindGameObjectsWithTag("SpawnZone");
        List<GameObject> ob = Gen.OfType<GameObject>().ToList();

        for (int i = 0; i < 5; i++)
        {

            var SpawnZone = UnityEngine.Random.Range(0, ob.Count);
            Vector3 tr = ob[SpawnZone].transform.position;
            Instantiate(GasCan, tr, Quaternion.identity);
            ob.RemoveAt(SpawnZone);


        }
    }


    private void SurviveMission()
    {

    }

    public void CheckClear()
    {
        switch (theMission)
        {
            case MissionList.Generator:

                if (CurrentGenerator == RequireGenerator)
                {
                    theManager.MissionClear();
                }
                break;

            case MissionList.Gascan:
                if (CurrentGascan == RequireGascan)
                {
                    theManager.MissionClear();
                }
                break;

            case MissionList.Survive:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && CurrentGascan == RequireGascan)
        {
            CheckClear();
        }
    }

}
