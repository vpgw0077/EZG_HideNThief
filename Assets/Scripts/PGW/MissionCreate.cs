using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MissionCreate : MonoBehaviour
{
    /*public enum MissionList
    {
        Generator,
        Gascan,
        Survive
    }
    public GameObject Generator;
    public int CurrentGenerator = 0;
    public int RequireGenerator = 3;

    public GameObject GasCan;
    public GameObject GasCanGenerator;
    public int CurrentGascan = 0;
    public int RequireGascan = 5;

    public MissionList theMission;
    public bool isClear;

    MissionManager theManager;
    Transform GascanGenTr;



    private void Start()
    {
        theManager = FindObjectOfType<MissionManager>();
        GascanGenTr = GameObject.FindGameObjectWithTag("GascanGen").GetComponent<Transform>();
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
        Instantiate(GasCanGenerator, GascanGenTr.position, GascanGenTr.rotation);
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
                    isClear = true;
                    theManager.MissionClear();
                }
                break;

            case MissionList.Gascan:
                if (CurrentGascan == RequireGascan)
                {
                    isClear = true;
                    theManager.MissionClear();
                }
                break;

            case MissionList.Survive:
                break;
        }
    }*/


}
