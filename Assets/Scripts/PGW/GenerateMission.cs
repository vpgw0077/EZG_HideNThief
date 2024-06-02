using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GenerateMission : Mission
{
    [SerializeField] private GameObject generator = null;

    [SerializeField] private int currentGen = 0;

    private readonly int requireGen = 3;

    private void Start()
    {
        Initialize();
        CreateGenerator();
        if (missionUI != null)
        {
            missionUI.MissionIcon.sprite = missionIcon;
            missionUI.UpdateMissionUI(currentGen, requireGen);

        }
    }

    protected override void Initialize()
    {
        currentGen = 0;

    }
    private void CreateGenerator()
    {
        GameObject[] spawnZone = GameObject.FindGameObjectsWithTag("SpawnZone");
        List<GameObject> spawnZoneList = spawnZone.OfType<GameObject>().ToList();

        for (int i = 0; i < requireGen; i++)
        {

            var spawnZoneIndex = Random.Range(0, spawnZoneList.Count);
            Vector3 tr = spawnZoneList[spawnZoneIndex].transform.position;
            Instantiate(generator, tr, Quaternion.identity);
            spawnZoneList.RemoveAt(spawnZoneIndex);


        }
    }

    public void UpdateMission()
    {
        currentGen++;
        missionUI.UpdateMissionUI(currentGen, requireGen);
        if (currentGen == requireGen)
        {
            MissionClear();
        }

    }

}
