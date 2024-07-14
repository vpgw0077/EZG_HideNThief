using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_FlashBang : Grenade, IGrenadeTrigger
{
    [SerializeField] private float flashBangRange = 0f;
    [SerializeField] private AudioClip flashBangClip = null;
    [SerializeField] private AudioSource greandeAudioPlayer = null;

    void Start()
    {
        StartCoroutine(IGrenadeCooking());
    }
    public IEnumerator IGrenadeCooking()
    {
        yield return new WaitForSeconds(countDown);
        GrenadeTrigger();


    }

    protected override void GrenadeTrigger()
    {
        ParticleSystem f_Effect = ParticlePooling.instance.GetF_Queue();
        f_Effect.transform.position = gameObject.transform.position;
        f_Effect.transform.parent = gameObject.transform;
        greandeAudioPlayer.clip = flashBangClip;
        greandeAudioPlayer.Play();
        Collider[] colls = Physics.OverlapSphere(transform.position, flashBangRange);

        foreach (var coll in colls)
        {
            var police = coll.GetComponent<IFlashBangRespond>();
            if (police != null)
            {
                police.RespondToFlashBang();

            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        onGroundAudioPlayer.clip = onGroundSfx[Random.Range(0, onGroundSfx.Length)];
        onGroundAudioPlayer.Play();
    }

}
