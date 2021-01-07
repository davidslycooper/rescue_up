using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    bool otherIsPilot;
    bool otherIsTutorial;

    bool roleReceived;
    bool tutorialReceived;

    public void JoinRoom(string roomNum)
    {
        Debug.Log("Joining Room number " + roomNum);
        PhotonNetwork.JoinRoom("Room" + roomNum);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        GetComponent<MenusMatchMaking>().JoinRoomError();
    }

    public void CreateRoom()
    {
        string roomNum = Random.Range(1000, 10000).ToString();
        string[] roomPropsInLobby = { "t", "p" };
        Hashtable customRoomProperties = new Hashtable() { { "t", GameManager.tutorial }, { "p", GameManager.pilot } };

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2, CustomRoomProperties = customRoomProperties, CustomRoomPropertiesForLobby = roomPropsInLobby };
        PhotonNetwork.CreateRoom("Room" + roomNum, roomOps);
        Debug.Log(roomNum);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        GetComponent<MenusMatchMaking>().CreateRoomError();
    }  
}
