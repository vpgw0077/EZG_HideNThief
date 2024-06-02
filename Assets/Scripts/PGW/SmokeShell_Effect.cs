using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmokeShell_Effect : MonoBehaviour
{
   [SerializeField] private ParticleSystem smokeEffect = null;

    private void OnEnable()
    {
        if (smokeEffect == null)
        {
            smokeEffect = GetComponent<ParticleSystem>();
        }

        StartCoroutine(PlaySmokeEffect());

    }

    private IEnumerator PlaySmokeEffect()
    {
        smokeEffect.Play();
        while (smokeEffect.isPlaying)
        {
            gameObject.transform.eulerAngles = Vector3.zero;
            yield return null;
        }
        StartCoroutine(DestroySmokeEffect());
    }
   private IEnumerator DestroySmokeEffect()
    {
        gameObject.transform.parent = null;
        ParticlePooling.instance.InsertQueue(smokeEffect);
        yield return null;

    }

}
