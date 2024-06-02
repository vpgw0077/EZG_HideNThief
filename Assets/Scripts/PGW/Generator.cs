using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{

    [SerializeField] protected AudioSource audioSource = null;
    [SerializeField] protected AudioClip audioClip = null;

    [SerializeField] protected GameObject generatorLight = null;

    [SerializeField] protected Animator generatorAnim = null;

    [SerializeField] protected float alarmRange = 100f;


    protected bool isActivate = false;



    protected void Start()
    {
        audioSource.clip = audioClip;
        
    }
    protected abstract void OperateGenerator();

}
