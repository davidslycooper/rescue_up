using UnityEngine;

public class Error : MonoBehaviour
{
    AudioSource speaker;

    public AudioClip warningEng;
    public AudioClip warningPt;

    float start;

    void Start()
    {
        start = Time.time;

        speaker = GetComponent<AudioSource>();
        speaker.clip = (GameManager.lang == 0) ? warningEng : warningPt;
        speaker.Play();
    }

    void Update()
    {
        if (Time.time > start + 3f && Input.anyKey)
        {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }
}
