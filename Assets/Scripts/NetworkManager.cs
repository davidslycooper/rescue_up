using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject manager;
    public GameObject quit;

    void Start()
    {
        if (GameManager.connError)
        {
            GetComponentInChildren<AudioSource>().Stop();

            quit.SetActive(true);
        }
        else if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            GetComponentInChildren<AudioSource>().Stop();
            manager.SetActive(true);
        } 
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the" + PhotonNetwork.CloudRegion + "server!");
        PhotonNetwork.AutomaticallySyncScene = false;
        GetComponentInChildren<AudioSource>().Stop();
        manager.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        GetComponentInChildren<AudioSource>().Stop();

        Debug.Log("Impossível estabelecer ligação. É possível que o servidor esteja cheio. Tente novamente mais tarde!");

        quit.SetActive(true);
    }

    //public override void OnErrorInfo(ErrorInfo errorInfo)
    //{
    //    Debug.Log("Impossível estabelecer ligação. É possível que o servidor esteja cheio. Tente novamente mais tarde!");

    //    quit.enabled = true;
    //}
}
