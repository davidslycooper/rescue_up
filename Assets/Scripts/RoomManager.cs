using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            if (((bool) PhotonNetwork.CurrentRoom.CustomProperties["t"]) != GameManager.tutorial) 
            {
                GameManager.roomNum = "";
                PhotonNetwork.LeaveRoom();
                GetComponent<MenusMatchMaking>().MissionError();
            }
            else if (((bool) PhotonNetwork.CurrentRoom.CustomProperties["p"]) == GameManager.pilot)
            {
                GameManager.roomNum = "";
                PhotonNetwork.LeaveRoom();
                GetComponent<MenusMatchMaking>().RoleSelectionError();
            }
            else
            {
                GameManager.roomNum = PhotonNetwork.CurrentRoom.Name.Remove(0,4);
                SceneManager.LoadScene(GameManager.waitMenuScene);
            }
        } else
        {
            GameManager.roomNum = PhotonNetwork.CurrentRoom.Name.Remove(0,4);
            SceneManager.LoadScene(GameManager.waitMenuScene);
        }
    }
}
