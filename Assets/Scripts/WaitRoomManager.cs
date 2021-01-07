using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitRoomManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    int playerCount;

    bool readyToStart;
    bool startingGame;

    public GameObject menu;
    public GameObject loading;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        PhotonNetwork.AutomaticallySyncScene = true;

        PV.RPC("RPC_Ready", RpcTarget.Others);

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;

        if (playerCount == 2)
        {
            readyToStart = true;
        } else
        {
            readyToStart = false;
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        Wait();
    }

    void Wait()
    {
        if (readyToStart)
        {
            if (startingGame)
            {
                return;
            }
            StartGame();
        }
    }

    void StartGame()
    {
        GetComponent<AudioSource>().Stop();
        menu.SetActive(false);
        loading.SetActive(true);

        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;

        if (GameManager.tutorial)
        {
            PhotonNetwork.LoadLevel(GameManager.tutorialMapScene);
        } 
        else
        {
            PhotonNetwork.LoadLevel(GameManager.randomMapScene);
        }
    }

    public void Cancel()
    {
        GameManager.roomNum = "";
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(GameManager.matchMakingScene);
    }

    [PunRPC]
    public void RPC_Ready()
    {
        PlayerCountUpdate();
    }
}