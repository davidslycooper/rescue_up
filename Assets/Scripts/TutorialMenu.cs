using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject loading;

    public void OnePlayer()
    {
        GetComponent<AudioSource>().Stop();
        menu.SetActive(false);
        loading.SetActive(true);
        SceneManager.LoadScene(GameManager.tutorialMapScene);
    }

    public void TwoPlayers()
    {
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene(GameManager.matchMakingScene);
    }

    public void Back()
    {
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene(GameManager.mainMenuScene);
    }
}
