using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang_Effect : MonoBehaviour
{
    public ParticleSystem Flash_Effect = null;

    private void OnEnable()
    {
        if (Flash_Effect == null)
        {
            Flash_Effect = GetComponent<ParticleSystem>();
        }

        Flash_Effect.Play();

    }
    IEnumerator DestroyEffect()
    {
        gameObject.transform.parent = null;
        ParticlePooling.instance.InsertF_Queue(Flash_Effect);
        yield return null;

    }

    private void Update()
    {
        if (Flash_Effect.isStopped)
        {
            StartCoroutine(DestroyEffect());
        }
    }
}
