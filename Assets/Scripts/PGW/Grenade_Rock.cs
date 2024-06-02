using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Rock : Grenade
{
    [SerializeField] private float rockSoundRange = 50f;
    protected override void GrenadeTrigger()
    {

        Collider[] colls = Physics.OverlapSphere(transform.position, rockSoundRange);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<EnemyAI>();
            if (police != null)
            {
                police.DrawAttention(transform.position);

            }
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("Enemy"))
        {
            GrenadeTrigger();
        }

        onGroundAudioPlayer.clip = onGroundSfx[Random.Range(0, onGroundSfx.Length)];
        onGroundAudioPlayer.Play();
    }

}
