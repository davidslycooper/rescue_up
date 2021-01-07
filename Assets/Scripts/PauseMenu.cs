using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    PhotonView PV;

    GameObject sub;
    CraftingPanel panel;

    public TutorialSteps tutorial;
    MusicController music;

    int state; // 0 = Main; 1 = Instructions; 2 = Controls; 3 = QuitPrompt; 4 = QuitWarn
    int option;
    int[] options;

    bool otherquit;

    public static bool paused;
    public GameObject pauseMenuUI;
    public GameObject pauseWaitUI;
    public GameObject controlsUI;
    public GameObject instructionsPilotUI;
    public GameObject instructionsSonarUI;
    public GameObject quitPromptUI;
    public GameObject quitWarnUI;

    AudioSource ambient;
    AudioSource speaker;

    public AudioClip swipe;
    public AudioClip block;
    public AudioClip transit;

    public AudioClip pauseMenuEng;
    public AudioClip pauseMenuPt;

    public AudioClip[] menuEng;
    public AudioClip[] menuPt;

    public AudioClip[] instructionsDefaultSonarEng;
    public AudioClip[] instructionsDefaultSonarPt;

    public AudioClip[] instructionsAltSonarEng;
    public AudioClip[] instructionsAltSonarPt;

    public AudioClip[] instructionsPilotEng;
    public AudioClip[] instructionsPilotPt;

    public AudioClip[] controlsEng;
    public AudioClip[] controlsPt;

    public AudioClip[] quitPromptEng;
    public AudioClip[] quitPromptPt;

    public AudioClip[] quitWarnEng;
    public AudioClip[] quitWarnPt;

    AudioClip pauseMenu;
    AudioClip[] menu;
    AudioClip[] instructions;
    AudioClip[] controls;
    AudioClip[] quitPrompt;
    AudioClip[] quitWarn;

    PlayerLog log;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        music = Camera.main.GetComponent<MusicController>();
        speaker = GetComponents<AudioSource>()[0];
        ambient = GetComponents<AudioSource>()[1];

        sub = GameObject.FindWithTag("Player");

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        option = -1;

        if (!GameManager.pilot)
        {
            panel = sub.GetComponentInChildren<CraftingPanel>();
        }

        options = new int[5] { 4, 7, 3, 3, 2 };

        if (GameManager.lang == 0)
        {
            pauseMenu = pauseMenuEng;
            menu = menuEng;
            controls = controlsEng;
            quitPrompt = quitPromptEng;
            quitWarn = quitWarnEng;

            if (GameManager.pilot)
            {
                instructions = instructionsPilotEng;
            } else
            {
                if (!GameManager.alternate)
                {
                    instructions = instructionsDefaultSonarEng;
                } else
                {
                    instructions = instructionsAltSonarEng;
                }
            } 
        }
        else
        {
            pauseMenu = pauseMenuPt;
            menu = menuPt;
            controls = controlsPt;
            quitPrompt = quitPromptPt;
            quitWarn = quitWarnPt;

            if (GameManager.pilot)
            {
                instructions = instructionsPilotPt;
            }
            else
            {
                if (!GameManager.alternate)
                {
                    instructions = instructionsDefaultSonarPt;
                }
                else
                {
                    instructions = instructionsAltSonarPt;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !otherquit)
        {
            if (paused)
            {
                if (state == 0)
                {
                    Resume();
                } else
                {
                    Back();
                }
            } else
            {
                Pause();
            }
        }
        if (paused)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!PhotonNetwork.InRoom)
                {
                    Time.timeScale = 1f;
                }
                option--;

                if (option < 0)
                {
                    option = 0;
                    AudioSource.PlayClipAtPoint(block, sub.transform.position + new Vector3(-0.01f, 0, 0));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(swipe, sub.transform.position + new Vector3(-0.01f, 0, 0));
                }
                if ((state == 0 && (GameManager.pilot || GameManager.tutorial) && option == 2) || (state == 1 && (GameManager.pilot) && option == 5))
                {
                    option--;
                }
                if (!PhotonNetwork.InRoom)
                {
                    Time.timeScale = 0f;
                }
                Say();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (!PhotonNetwork.InRoom)
                {
                    Time.timeScale = 1f;
                }
                option++;

                if (option > options[state] - 1)
                {
                    option = options[state] - 1;
                    AudioSource.PlayClipAtPoint(block, sub.transform.position + new Vector3(0.01f, 0, 0));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(swipe, sub.transform.position + new Vector3(0.01f, 0, 0));
                }
                if ((state == 0 && (GameManager.pilot || GameManager.tutorial) && option == 2) || (state == 1 && (GameManager.pilot) && option == 5))
                {
                    option++;
                }
                if (!PhotonNetwork.InRoom)
                {
                    Time.timeScale = 0f;
                }
                Say();
            }
            else if (Input.GetKeyUp(KeyCode.Return))
            {
                speaker.Stop();

                if (option >= 0)
                {
                    Do();
                }
                else
                {
                    Say();
                }
            }
        }
    }

    void Pause()
    {
        log.Pause();

        Transit();
        option = -1;

        if (!GameManager.pilot)
        {
            panel.Pause();
        }

        ambient.Play();

        if (GameManager.tutorial)
        {
            tutorial.Pause();
        }
        else
        {
            music.Pause();
        }

        speaker.clip = pauseMenu;
        speaker.Play();

        state = 0;

        Cursor.visible = true;

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_Pause", RpcTarget.Others);
        }

        pauseMenuUI.SetActive(true);

        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0f;
        }
        paused = true;
    }

    public void Resume()
    {
        log.Resume();

        Transit();
        option = -1;

        if (!GameManager.pilot)
        {
            sub.GetComponentInChildren<CraftingPanel>().Unpause();
        }

        ambient.Stop();

        if (GameManager.tutorial)
        {
            tutorial.Unpause();
        }
        else
        {
            music.Unpause();
        }

        Cursor.visible = false;

        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_Resume", RpcTarget.Others);
        }

        pauseMenuUI.SetActive(false);

        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1f;
        }
        paused = false;
    }

    public void Instructions()
    {
        log.Instructions();

        Transit();
        option = -1;

        state = 1;

        pauseMenuUI.SetActive(false);

        if (GameManager.pilot)
        {
            instructionsPilotUI.SetActive(true);
        }
        else
        {
            instructionsSonarUI.SetActive(true);
        }
    }

    public void Controls()
    {
        Transit();
        option = -1;

        state = 2;

        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void QuitPrompt()
    {
        Transit();

        state = 3;

        speaker.clip = quitPrompt[0];
        speaker.Play();

        option = 0;

        pauseMenuUI.SetActive(false);

        quitPromptUI.SetActive(true);
    }

    public void QuitWarn()
    {
        Transit();

        GameManager.roomNum = "";
        PhotonNetwork.LeaveRoom();

        if (!GameManager.pilot)
        {
            panel.Pause();
        }

        ambient.Play();

        if (GameManager.tutorial)
        {
            tutorial.Pause();
        }
        else
        {
            music.Pause();
        }

        state = 4;

        Cursor.visible = true;

        Time.timeScale = 0f;
        paused = true;

        speaker.clip = quitWarn[0];
        speaker.Play();

        option = 0;

        otherquit = true;

        if (paused)
        {
            switch (state)
            {
                case 0:
                    pauseMenuUI.SetActive(false);
                    break;
                case 1:
                    if (GameManager.pilot)
                    {
                        instructionsPilotUI.SetActive(false);
                    }
                    else
                    {
                        instructionsSonarUI.SetActive(false);
                    }
                    break;
                case 2:
                    controlsUI.SetActive(false);
                    break;
                case 3:
                    quitPromptUI.SetActive(false);
                    break;
            }
        }

        quitWarnUI.SetActive(true);
    }

    public void Back()
    {
        Transit();

        option = -1;

        switch (state)
        {
            case 1:
                if (GameManager.pilot)
                {
                    instructionsPilotUI.SetActive(false);
                }
                else
                {
                    instructionsSonarUI.SetActive(false);
                }
                break;
            case 2:
                controlsUI.SetActive(false);
                break;
            case 3:
                quitPromptUI.SetActive(false);
                break;
        }

        state = 0;
        pauseMenuUI.SetActive(true);
    }

    public void Quit()
    {
        Transit();

        if (PhotonNetwork.InRoom && !otherquit) 
        {
            PV.RPC("RPC_Quit", RpcTarget.Others);
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
        }
        ToMainMenu();
    }

    public void ToMainMenu()
    {
        log.QuitFromPause();

        paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.loadingScene);
    }

    public void DefaultControls()
    {
        log.ChangedControls(false);

        GameManager.alternate = false;

        if (GameManager.lang == 0)
        {
            instructions = instructionsDefaultSonarEng;
        }
        else
        {
            instructions = instructionsDefaultSonarPt;
        }

        Back();
    }

    public void AlternateControls()
    {
        log.ChangedControls(true);

        GameManager.alternate = true;

        if (GameManager.lang == 0)
        {
            instructions = instructionsAltSonarEng;
        }
        else
        {
            instructions = instructionsAltSonarPt;
        }

        Back();
    }

    void Transit()
    {
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1f;
        }
        AudioSource.PlayClipAtPoint(transit, sub.transform.position);
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0f;
        }
    }

    [PunRPC]
    public void RPC_Pause()
    {
        pauseWaitUI.SetActive(true);
    }

    [PunRPC]
    public void RPC_Resume()
    {
        pauseWaitUI.SetActive(false);
    }

    [PunRPC]
    public void RPC_Quit()
    {
        QuitWarn();
    }

    void Say() {
        switch(state)
        {
            case 0:
                speaker.clip = menu[option];
                break;
            case 1:
                speaker.clip = instructions[option];
                break;
            case 2:
                speaker.clip = controls[option];
                break;
            case 3:
                speaker.clip = quitPrompt[option];
                break;
            case 4:
                speaker.clip = quitWarn[option];
                break;
        }
        speaker.Play();
    }

    void Do()
    {
        switch (state)
        {
            case 0:
                switch (option)
                {
                    case 0:
                        Resume();
                        break;
                    case 1:
                        Instructions();
                        break;
                    case 2:
                        Controls();
                        break;
                    case 3:
                        QuitPrompt();
                        break;
                }
                break;
            case 1:
                if (option < 6)
                {
                    Say();
                } else
                {
                    Back();
                }
                break;
            case 2:
                switch (option)
                {
                    case 0:
                        DefaultControls();
                        break;
                    case 1:
                        AlternateControls();
                        break;
                    case 2:
                        Back();
                        break;
                }
                break;
            case 3:
                switch (option)
                {
                    case 0:
                        Say();
                        break;
                    case 1:
                        Quit();
                        break;
                    case 2:
                        Back();
                        break;
                }
                break;
            case 4:
                switch (option)
                {
                    case 0:
                        Say();
                        break;
                    case 1:
                        Quit();
                        break;
                }
                break;
        }
    }
}
