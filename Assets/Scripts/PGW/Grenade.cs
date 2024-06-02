using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grenade : MonoBehaviour
{
    protected float countDown = 2f;

   [SerializeField] protected AudioSource onGroundAudioPlayer;
   [SerializeField] protected AudioClip[] onGroundSfx;

    protected abstract void GrenadeTrigger();

    protected abstract void OnCollisionEnter(Collision collision);


}
