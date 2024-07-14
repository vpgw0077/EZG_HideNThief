using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] walkSound;
    [SerializeField] private AudioSource soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
    }
    private void FootStepSound()
    {
        soundPlayer.clip = walkSound[Random.Range(0, walkSound.Length)];
        soundPlayer.Play();
    }
}
