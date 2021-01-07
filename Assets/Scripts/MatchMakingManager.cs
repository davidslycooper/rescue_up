using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class MatchMakingManager : MonoBehaviour
{
    TMP_InputField roomNum;

    void Awake()
    {
        roomNum = GameObject.FindWithTag("RoomNum").GetComponent<TMP_InputField>();

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Create()
    {
        GetComponent<MatchMaking>().CreateRoom();
    }

    public void Join()
    {
        GetComponent<MatchMaking>().JoinRoom(roomNum.text);
    }

    public void Back()
    {
        SceneManager.LoadScene(GameManager.mainMenuScene);
    }
}
