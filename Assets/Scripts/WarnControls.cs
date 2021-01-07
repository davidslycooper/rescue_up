using UnityEngine;
using UnityEngine.SceneManagement;

public class WarnControls : MonoBehaviour
{
    public void Default()
    {
        GameManager.warned = true;
        GameManager.alternate = false;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else 
        {
            SceneManager.LoadScene(GameManager.matchMakingScene);
        } 
    }

    public void Alternative()
    {
        GameManager.warned = true;
        GameManager.alternate = true;

        if (GameManager.inOptions)
        {
            SceneManager.LoadScene(GameManager.menuOptionsScene);
        }
        else
        {
            SceneManager.LoadScene(GameManager.matchMakingScene);
        }
    }
}
