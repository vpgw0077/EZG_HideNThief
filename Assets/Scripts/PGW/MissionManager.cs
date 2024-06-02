using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private Mission[] MissionList;
    private void Awake()
    {
        MissionList = gameObject.GetComponentsInChildren<Mission>(true);
        var r = Random.Range(0, MissionList.Length);
        MissionList[r].gameObject.SetActive(true);
    }

}
