using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayStartPilot()
    {
        GameManager.pilot = true;
        GameManager.tutorial = false;
        SceneManager.LoadScene(GameManager.matchMakingScene);
    }

    public void TutorialStartPilot()
    {
        GameManager.pilot = true;
        GameManager.tutorial = true;
        SceneManager.LoadScene(GameManager.matchMakingScene);
    }

    public void PlayStartEngin()
    {
        GameManager.pilot = false;
        GameManager.tutorial = false;
        if (GameManager.warned)
        {
            SceneManager.LoadScene(GameManager.matchMakingScene);
        }
        else
        {
            SceneManager.LoadScene(GameManager.warnControlsScene);
        }
    }

    public void TutorialStartEngin()
    {
        GameManager.pilot = false;
        GameManager.tutorial = true;
        if (GameManager.warned)
        {
            SceneManager.LoadScene(GameManager.matchMakingScene);
        } 
        else
        {
            SceneManager.LoadScene(GameManager.warnControlsScene);
        }
    }

    public void Options()
    {
        GameManager.inOptions = true;
        SceneManager.LoadScene(GameManager.menuOptionsScene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
