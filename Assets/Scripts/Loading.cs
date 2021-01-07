using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    AudioSource speaker;

    public AudioClip loadingEng;
    public AudioClip loadingPt;

    void Start()
    {
        if (GameManager.narrator && !GameManager.connError)
        {
            speaker = GetComponent<AudioSource>();
            speaker.clip = (GameManager.lang == 0) ? loadingEng : loadingPt;
            speaker.Play();
        }
    }
}
