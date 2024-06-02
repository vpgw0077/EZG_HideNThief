using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Mission : MonoBehaviour
{
    [SerializeField] protected Sprite missionIcon = null;
    [SerializeField] protected Animator anim = null;


    protected MissionUI missionUI = null;

    protected abstract void Initialize();

    protected void Awake()
    {
        missionUI = FindObjectOfType<MissionUI>();
    }
    public void MissionClear()
    {
        anim.SetTrigger("DoorOpen");
    }

}
