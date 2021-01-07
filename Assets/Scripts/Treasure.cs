using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{

    AudioSource dolphinAS;
    public AudioClip[] dolphinSounds;

    public float delaySound;
    float nextSound;

    void Start()
    {
        dolphinAS = GetComponent<AudioSource>();
    }

    public void Dolphin()
    {
        if (!dolphinAS.isPlaying && Time.time > nextSound)
        {
            dolphinAS.clip = dolphinSounds[Random.Range(0, dolphinSounds.Length)];

            dolphinAS.Play();
            nextSound = Time.time + delaySound;
        }
        
    }
}
