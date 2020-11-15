using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeShell_Effect : MonoBehaviour
{
    public ParticleSystem Smoke_Effect = null;

    private void OnEnable()
    {
        if(Smoke_Effect == null)
        {
            Smoke_Effect = GetComponent<ParticleSystem>();
        }

        Smoke_Effect.Play();

    }
    IEnumerator DestroyEffect()
    {
        yield return null;
        gameObject.transform.parent = null;
        ParticlePooling.instance.InsertQueue(Smoke_Effect);
        
    }

    private void Update()
    {
        if (Smoke_Effect.isStopped)
        {
            StartCoroutine(DestroyEffect());
        }
    }
}
