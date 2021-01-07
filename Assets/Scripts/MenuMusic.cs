using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic menuMusic;

    AudioSource music;
    public AudioClip[] musics;

    int num;

    void Awake()
    {
        if (menuMusic == null)
        {
            menuMusic = this;

            num = 0;

            music = GetComponent<AudioSource>();
            NextMusic();

            DontDestroyOnLoad(this.gameObject);
        }
        else if (menuMusic != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!music.isPlaying)
        {
            NextMusic();
        }
        if (SceneManager.GetActiveScene().buildIndex > GameManager.waitMenuScene)
        {
            Destroy(this.gameObject);
        }
    }

    void NextMusic()
    {
        music.clip = musics[num];
        num = (num + 1) % musics.Length;
        music.Play();
    }
}
