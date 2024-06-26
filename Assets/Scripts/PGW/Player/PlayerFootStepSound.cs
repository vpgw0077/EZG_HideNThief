using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] WalkSound = null;
    [SerializeField] private AudioSource SoundPlayer = null;


    private readonly float walkSoundPeriod = 0.6f;
    private readonly float runSoundPeriod = 0.27f;
    private float currentSoundTime = 0;

    private void Awake()
    {
        SoundPlayer = GetComponent<AudioSource>();
    }

    public void PlayWalkSound()
    {
        currentSoundTime += Time.deltaTime;
        if (currentSoundTime >= walkSoundPeriod)
        {
            currentSoundTime = 0;
            SoundPlayer.clip = WalkSound[Random.Range(0, WalkSound.Length)];
            SoundPlayer.Play();

        }
    }

    public void PlayRunSound()
    {
        currentSoundTime += Time.deltaTime;
        if (currentSoundTime >= runSoundPeriod)
        {
            currentSoundTime = 0;
            SoundPlayer.clip = WalkSound[Random.Range(0, WalkSound.Length)];
            SoundPlayer.Play();

        }
    }
}
