using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectDetect : MonoBehaviourPunCallbacks
{
    public static DisconnectDetect conn;

    void Start()
    {
        if (conn == null)
        {
            conn = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (conn != this)
        {
            Destroy(gameObject);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager.connError = true;

        GameObject.Find("PlayerLog").GetComponent<PlayerLog>().ConnectionError();

        PauseMenu.paused = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(GameManager.loadingScene);
    }

    //void OnConnectionFail(DisconnectCause cause)
    //{
    //    GameManager.connError = true;

    //    GameObject.Find("PlayerLog").GetComponent<PlayerLog>().ConnectionError();

    //    PauseMenu.paused = false;
    //    Time.timeScale = 1f;

    //    SceneManager.LoadScene(GameManager.loadingScene);
    //}
}