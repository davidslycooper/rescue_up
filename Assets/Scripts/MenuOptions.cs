using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    public void Language()
    {
        SceneManager.LoadScene(GameManager.languageMenuScene);
    }

    public void Controls()
    {
        SceneManager.LoadScene(GameManager.warnControlsScene);
    }

    public void Narration()
    {
        SceneManager.LoadScene(GameManager.narratorOptionsScene);
    }

    public void Back()
    {
        GameManager.inOptions = false;
        SceneManager.LoadScene(GameManager.mainMenuScene);
    }
}