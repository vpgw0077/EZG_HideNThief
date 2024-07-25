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

        StartCoroutine(PlayEffect());

    }
    IEnumerator DestroyEffect()
    {
        gameObject.transform.parent = null;
        ParticlePooling.instance.InsertF_Queue(Flash_Effect);
        yield return null;

    }
    IEnumerator PlayEffect()
    {
        Flash_Effect.Play();
        while (Flash_Effect.isPlaying)
        {
            yield return null;
        }
        StartCoroutine(DestroyEffect());
    }

}
