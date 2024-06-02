using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private AudioClip sirenSfx = null;
    [SerializeField] private AudioSource audioPlayer = null;

    [SerializeField] private float SirenRange = 70f;

    private void Start()
    {
        audioPlayer.clip = sirenSfx;
    }
    private void SirenTrigger()
    {
        audioPlayer.Play();
        Collider[] colls = Physics.OverlapSphere(transform.position, SirenRange);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<EnemyAI>();
            if (police != null)
            {
                police.ChangeState(EnemyState.Chase);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SirenTrigger();

        }
    }

}
