using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene(GameManager.loadingScene);
    }
}
