using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public Sound[] Bgm;
    public AudioSource bgmPlayer;

    public Sound ChaseBgm;
    public AudioSource ChasebgmPlayer;

    public Sound[] SfxSounds;
    public AudioSource[] sfxPlayer;

    int random;

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
        ChasebgmPlayer.clip = ChaseBgm.clip;
        ChasebgmPlayer.Play();
    }

    public void PlaySE(string _soundName)
    {

        for (int i = 0; i < SfxSounds.Length; i++)
        {
            if (_soundName == SfxSounds[i].soundName)
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
                return;
            }
        }

    }
    private void Update()
    {
        if (GameController.instance.PoliceAware.Count == 0)
        {
            CheckBGM_End();
        }

        PlayChaseBgm();


    }

    private void PlayChaseBgm()
    {
        if (GameController.instance.PoliceAware.Count != 0)
        {
            bgmPlayer.Stop();
            ChasebgmPlayer.volume += 0.001f;
            if (ChasebgmPlayer.volume >= 0.1f)
            {
                ChasebgmPlayer.volume = 0.1f;
            }

            

        }
        else if (GameController.instance.PoliceAware.Count == 0)
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
        if (!bgmPlayer.isPlaying)
        {
            random = UnityEngine.Random.Range(0, Bgm.Length);
            bgmPlayer.clip = Bgm[random].clip;
            bgmPlayer.Play();
        }
    }

    public void PlayBGM()
    {
        random = UnityEngine.Random.Range(0, Bgm.Length);
        bgmPlayer.clip = Bgm[random].clip;
        bgmPlayer.Play();
    }

}
