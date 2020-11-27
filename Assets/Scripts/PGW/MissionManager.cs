using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public GameObject[] MissionList;
    public Animator DoorAnim;

    private void Awake()
    {
        var r = Random.Range(0, 2);
        Instantiate(MissionList[r], transform.position, Quaternion.identity);
    }
    public void MissionClear()
    {
        DoorAnim.SetTrigger("DoorOpen");

    }

}
