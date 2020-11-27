using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public AudioSource sfxPlayer;
    public AudioClip TenseBgm;

    public bool isDetected;

    private void Start()
    {
        sfxPlayer.clip = TenseBgm;
    }
    private void Update()
    {
        if (isDetected)
        {
            sfxPlayer.volume += 0.005f;
            if (sfxPlayer.volume >= 0.2f)
            {
                sfxPlayer.volume = 0.2f;
            }
        }
        else
        {
            sfxPlayer.volume -= 0.001f;
            if (sfxPlayer.volume <= 0)
            {
                sfxPlayer.volume = 0;
            }
        }

        if(GameController.instance.PoliceAware.Count != 0)
        {
            isDetected = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GameController.instance.PoliceAware.Count == 0)
            {

                isDetected = true;
                if (!sfxPlayer.isPlaying)
                {
                    sfxPlayer.Play();
                }

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GameController.instance.PoliceAware.Count == 0)
            {

                isDetected = false;

            }
        }
    }

}
