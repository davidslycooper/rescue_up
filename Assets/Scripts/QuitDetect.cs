using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitDetect : MonoBehaviour
{
    public static QuitDetect quit;

    void Start()
    {
        if (quit == null)
        {
            quit = this;
            DontDestroyOnLoad(this.gameObject);
            GameObject.Find("PlayerLog").GetComponent<PlayerLog>().StartGame();
        }
        else if (quit != this)
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        GameObject.Find("PlayerLog").GetComponent<PlayerLog>().QuitGame();
    }
}
