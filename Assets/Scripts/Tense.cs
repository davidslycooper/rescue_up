using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tense : MonoBehaviour
{
    AudioSource tenseAmbient;

    void Start()
    {
        tenseAmbient = GetComponent<AudioSource>();
    }

    public void TenseAmbient()
    {
        if (!tenseAmbient.isPlaying)
        {
            tenseAmbient.Play();
        }
    }
}
