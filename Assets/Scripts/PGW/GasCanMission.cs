using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GasCanMission : Mission
{
    [SerializeField] private GameObject gasCanGenerator = null;
    [SerializeField] private GameObject gasCan = null;
    [SerializeField] private Transform spawnPosition = null;

    private bool isGetAllGasCan = false;
    public bool IsGetAllGasCan
    {
        get { return isGetAllGasCan; }
        set { isGetAllGasCan = value; }
    }

    private int currentGasCan = 0;

    public int CurrentGasCan
    {
        get { return currentGasCan; }
        set { currentGasCan = value; }
    }

    private readonly int requireGasCan = 3;

    private void Start()
    {
        Initialize();
        CreateGasCan();
        if (missionUI != null)
        {
            missionUI.MissionIcon.sprite = missionIcon;
            missionUI.UpdateMissionUI(CurrentGasCan, requireGasCan);

        }


    }

    protected override void Initialize()
    {
        CurrentGasCan = 0;
        isGetAllGasCan = false;
    }
    private void CreateGasCan()
    {
        Instantiate(gasCanGenerator, spawnPosition.position, spawnPosition.rotation);
        GameObject[] gasCanSpawnZone = GameObject.FindGameObjectsWithTag("SpawnZone");
        List<GameObject> ob = gasCanSpawnZone.OfType<GameObject>().ToList();

        for (int i = 0; i < requireGasCan; i++)
        {

            var SpawnZone = Random.Range(0, ob.Count);
            Vector3 tr = ob[SpawnZone].transform.position;
            Instantiate(gasCan, tr, Quaternion.identity);
            ob.RemoveAt(SpawnZone);


        }
    }
    public void GetGasCan()
    {
        CurrentGasCan++;
        missionUI.UpdateMissionUI(CurrentGasCan, requireGasCan);
        if (CurrentGasCan == requireGasCan) IsGetAllGasCan = true;
    }

}
