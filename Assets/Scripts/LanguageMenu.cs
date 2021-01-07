using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageMenu : MonoBehaviour
{
    public void English()
    {
        GameManager.lang = 0;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else // starting game
        {
            SceneManager.LoadScene(GameManager.narratorOptionsScene);
        }
    }

    public void Portuguese()
    {
        GameManager.lang = 1;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else // starting game
        {
            SceneManager.LoadScene(GameManager.narratorOptionsScene);
        }
    }
}
