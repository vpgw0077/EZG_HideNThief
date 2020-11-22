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

    public Sound Bgm;
    public AudioSource bgmPlayer;

    public Sound[] SfxSounds;
    public AudioSource[] sfxPlayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBGM();
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

    public void PlayBGM()
    {
        bgmPlayer.clip = Bgm.clip;
        bgmPlayer.Play();
    }

}
