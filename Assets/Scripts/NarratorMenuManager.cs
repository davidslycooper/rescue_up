using UnityEngine.SceneManagement;
using UnityEngine;

public class NarratorMenuManager : MonoBehaviour
{
    public void Enable()
    {
        GameManager.narrator = true;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else
        {
            SceneManager.LoadScene(GameManager.loginMenuScene);
        }
    }

    public void Disable()
    {
        GameManager.narrator = false;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else
        {
            SceneManager.LoadScene(GameManager.loginMenuScene);
        }
    }
}
