using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneManager : MonoBehaviour
{
    AudioSource speaker;

    public AudioClip warningEng;
    public AudioClip warningPt;

    void Start()
    {
        speaker = GetComponents<AudioSource>()[0];

        if (!PhotonNetwork.IsConnected || !GameManager.alreadyLoaded)
        {
            if (GameManager.narrator)
            {
                speaker.clip = (GameManager.lang == 0) ? warningEng : warningPt;
                speaker.Play();
            }
            GameManager.Load();
            GameManager.alreadyLoaded = true;
        }
        else
        {
            SceneManager.LoadScene(GameManager.mainMenuScene);
        }
    }

    void Update()
    {
        if (Input.anyKey || (GameManager.narrator && !speaker.isPlaying))
        {
            SceneManager.LoadScene(GameManager.mainMenuScene);
        }
    }
}
