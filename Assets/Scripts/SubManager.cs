using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SubManager : MonoBehaviour
{
    PhotonView PV;

    public AudioSource speaker;
    public AudioClip[] states; // 0 = Mech Hand; 1 = Signer; 2 = Cannons; 3 = Default
    public AudioClip batteryDec; // Som
    public AudioClip outOfAmmo; // Som
    public AudioClip catchSound;

    public int state;

    SubLightManager light;
    PlayerController sub;
    CraftingPanel panel;

    public bool topLoaded;
    public bool botLoaded;
    public bool flareLoaded;

    Vector3 startPoint;
    public bool treasureCaught;
    bool simulating;
    float simulateWait;

    float stateChangeTime;

    PlayerLog log;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        light = GetComponentInChildren<SubLightManager>();
        sub = GetComponent<PlayerController>();
        panel = GetComponentInChildren<CraftingPanel>();

        speaker = GetComponent<AudioSource>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        state = 3;
        stateChangeTime = Time.time;
    }

    void Update()
    {
        if (treasureCaught && Vector3.Distance(transform.position, startPoint) < 3)
        {
            Win();
        }
        if (simulating && Time.time > simulateWait)
        {
            transform.position = new Vector3(-5f, -9f);
            simulating = false;
        }
    }

    void Win()
    {
        if (GameManager.tutorial)
        {
            if(PhotonNetwork.InRoom)
            {
                PV.RPC("RPC_Win", RpcTarget.Others);
            }
            GameObject.Find("PhotonTutorialGeneration").GetComponent<TutorialSteps>().Win();
        }
        else
        {
            PV.RPC("RPC_Win", RpcTarget.Others);

            log.Win();

            Cursor.visible = true;
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(GameManager.winScene);
        }
        Debug.Log("Win");
    }

    public void SetStartPoint(Vector3 start)
    {
        startPoint = start;
    }

    public void OutOfAmmo()
    {
        speaker.clip = outOfAmmo;
        speaker.Play();
    }

    public void ChangeState(int changeState)
    {
        float time = Time.time - stateChangeTime;

        if (!GameManager.tutorial)
        {
            log.ChangeState(state, changeState, time);
        }

        state = changeState;

        speaker.clip = states[changeState];
        speaker.Play();

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_ChangeState", RpcTarget.Others, changeState);
        }
    }

    public void SimulateCatch(int qty, int type)
    {
        for (int i = 0; i < qty; i++)
        {
            panel.CatchMineral(type);
        }
    }

    public void SimulateFire()
    {
        sub.SimulateShoot();
        simulateWait = Time.time + 4f;
        simulating = true;
    }

    public void CatchSound()
    {
        speaker.clip = catchSound;
        speaker.Play();
    }

    public void Die(bool battery, int id = 0)
    {
        GameManager.deathCause = battery;

        if (GameManager.tutorial)
        {
            if (PhotonNetwork.InRoom)
            {
                PV.RPC("RPC_Die", RpcTarget.Others, battery, id);
            }
            GameObject.Find("PhotonTutorialGeneration").GetComponent<TutorialSteps>().Die();
        }
        else
        {
            PV.RPC("RPC_Die", RpcTarget.Others, battery, id);

            log.Die(id, transform.position);

            Cursor.visible = true;
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(GameManager.deathScene);
        }
    }

    public void DecBattery()
    {
        //light.DecBattery();
        speaker.clip = batteryDec;
        speaker.Play();

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_DecBattery", RpcTarget.Others);
        }
    }

    public void ChargeBattery(int power)
    {
        light.ChargeBattery(power);

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_ChargeBattery", RpcTarget.Others, power);
        }
    }

    public void LoadSigner()
    {
        flareLoaded = true;

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_LoadSigner", RpcTarget.Others);
        }
    }

    public void LoadCannon(bool top)
    {
        if (top)
        {
            topLoaded = true;
        } else
        {
            botLoaded = true;
        }
        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_LoadCannon", RpcTarget.Others, top);
        }
    }

    public void FlareSpend()
    {
        if (!GameManager.tutorial)
        {
            log.Sign(transform.position);
        }

        flareLoaded = false;
        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_FlareSpend", RpcTarget.Others);
        }
    }

    public void TorpedoSpend(bool top)
    {
        if (!GameManager.tutorial)
        {
            log.Shoot(top, transform.position);
        }

        if (top)
        {
            topLoaded = false;
        } else
        {
            botLoaded = false;
        }
        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_TorpedoSpend", RpcTarget.Others, top);
        }
    }

    public void CatchTreasure()
    {
        treasureCaught = true;
        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_CatchTreasure", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void RPC_Win()
    {
        if (GameManager.tutorial)
        {
            GameObject.Find("PhotonTutorialGeneration").GetComponent<TutorialSteps>().Win();
        }
        else
        {
            log.Win();

            Cursor.visible = true;
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(GameManager.winScene);
        }
    }

    [PunRPC]
    public void RPC_ChangeState(int changeState)
    {
        state = changeState;

        speaker.clip = states[changeState];
        speaker.Play();

        panel.ChangeState(changeState == 3);
    }

    [PunRPC]
    public void RPC_Die(bool battery, int id)
    {
        GameManager.deathCause = battery;

        if (GameManager.tutorial)
        {
            GameObject.Find("PhotonTutorialGeneration").GetComponent<TutorialSteps>().Die();
        }
        else
        {
            log.Die(id, transform.position);

            Cursor.visible = true;
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(GameManager.deathScene);
        }
    }

    [PunRPC]
    public void RPC_DecBattery()
    {
        light.DecBattery();
        speaker.clip = batteryDec;
        speaker.Play();
    }

    [PunRPC]
    public void RPC_ChargeBattery(int power)
    {
        light.ChargeBattery(power);
    }

    [PunRPC]
    public void RPC_LoadSigner()
    {
        flareLoaded = true;
    }

    [PunRPC]
    public void RPC_LoadCannon(bool top)
    {
        if (top)
        {
            topLoaded = true;
        } else
        {
            botLoaded = true;
        }
    }

    [PunRPC]
    public void RPC_FlareSpend()
    {
        flareLoaded = false;
    }

    [PunRPC]
    public void RPC_TorpedoSpend(bool top)
    {
        if (top)
        {
            topLoaded = false;
        }
        else
        {
            botLoaded = false;
        }
    }

    [PunRPC]
    public void RPC_CatchTreasure()
    {
        treasureCaught = true;
    }
}
