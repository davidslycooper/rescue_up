using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource music;
    AudioSource[] sources;
    public AudioClip[] musics;
    int musicSel;

    bool newMusic;
    float newMusicTime;

    void Start()
    {
        sources = GetComponentsInChildren<AudioSource>();

        music = sources[2];

        if (GameManager.pilot)
        {
            musicSel = musics.Length;
            music.volume = 0.3f;
        } else
        {
            musicSel = musics.Length - 2;
        }

        music.clip = musics[UnityEngine.Random.Range(0, musicSel)];
        music.Play();
    }

    void Update()
    {
        if (!music.isPlaying && !newMusic)
        {
            NewMusic();
        }
        if (newMusic && Time.time > newMusicTime)
        {
            PlayMusic();
        }
    }

    void NewMusic()
    {
        music.clip = musics[UnityEngine.Random.Range(0, musicSel)];

        newMusicTime = Time.time + 15f;
        newMusic = true;
    }

    void PlayMusic()
    {
        music.Play();
        newMusic = false;
    }

    public void Pause()
    {
        foreach (AudioSource source in sources)
        {
            source.Pause();
        }
    }

    public void Unpause()
    {
        foreach (AudioSource source in sources)
        {
            source.UnPause();
        }
    }
}
