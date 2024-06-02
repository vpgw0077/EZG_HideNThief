using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SfxType
{
    grenadeThrow,
    itemGrab
}
[System.Serializable]
public class SfxSound
{
    public SfxType sfxType;
    public AudioClip clip;
}
[System.Serializable]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [SerializeField] private AudioClip[] usualBgmClip = null;
    [SerializeField] private AudioSource usualBgmPlayer = null;

    [Space]
    [Space]
    [SerializeField] private AudioClip chaseBgmClip = null;
    [SerializeField] private AudioSource ChasebgmPlayer = null;

    [Space]
    [Space]
    [SerializeField] private SfxSound[] SfxSounds = null;
    [SerializeField] private AudioSource[] sfxPlayer = null;

    private int randomClipIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    private void Start()
    {
        PlayBGM();
        ChasebgmPlayer.clip = chaseBgmClip;
        ChasebgmPlayer.Play();
    }

    public void PlaySE(SfxType type)
    {

        for (int i = 0; i < SfxSounds.Length; i++)
        {
            if (type == SfxSounds[i].sfxType)
            {
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = SfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                }

            }
        }

    }
    private void Update()
    {
        if (GameController.instance.awarePoliceList.Count == 0)
        {
            CheckBGM_End();
        }

        PlayChaseBgm();


    }

    private void PlayChaseBgm()
    {
        if (GameController.instance.awarePoliceList.Count != 0)
        {
            usualBgmPlayer.Stop();
            ChasebgmPlayer.volume += 0.001f;
            if (ChasebgmPlayer.volume >= 0.1f)
            {
                ChasebgmPlayer.volume = 0.1f;
            }



        }
        else if (GameController.instance.awarePoliceList.Count == 0)
        {
            ChasebgmPlayer.volume -= 0.001f;
            if (ChasebgmPlayer.volume <= 0)
            {
                ChasebgmPlayer.volume = 0;
            }
        }

    }

    private void CheckBGM_End()
    {
        if (!usualBgmPlayer.isPlaying)
        {
            randomClipIndex = UnityEngine.Random.Range(0, usualBgmClip.Length);
            usualBgmPlayer.clip = usualBgmClip[randomClipIndex];
            usualBgmPlayer.Play();
        }
    }

    public void PlayBGM()
    {
        randomClipIndex = UnityEngine.Random.Range(0, usualBgmClip.Length);
        usualBgmPlayer.clip = usualBgmClip[randomClipIndex];
        usualBgmPlayer.Play();
    }

}
