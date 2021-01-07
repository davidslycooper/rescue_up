using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class MenusMatchMaking : MonoBehaviour
{
    int scene;
    public AudioClip[] menuOptionsEng;
    public AudioClip[] menuOptionsPt;

    AudioClip[] menuOptions;

    public AudioClip createRoomErrorEng;
    public AudioClip createRoomErrorPt;
    AudioClip createRoomError;

    public AudioClip joinRoomErrorEng;
    public AudioClip joinRoomErrorPt;

    AudioClip joinRoomError;

    public AudioClip missionErrorTutEng;
    public AudioClip missionErrorTutPt;

    AudioClip missionErrorTut;

    public AudioClip missionErrorEng;
    public AudioClip missionErrorPt;

    AudioClip missionError;

    public AudioClip roleSelectionErrorEng;
    public AudioClip roleSelectionErrorPt;

    AudioClip roleSelectionError;

    public AudioClip warnInsertingEng;
    public AudioClip warnInsertingPt;
    AudioClip warnInserting;

    public AudioClip warnJoinEng;
    public AudioClip warnJoinPt;
    AudioClip warnJoin;

    public TextMeshProUGUI warnings;

    public AudioClip[] numsEng;
    public AudioClip[] numsPt;
    AudioClip[] nums;

    public AudioClip eraseEng;
    public AudioClip erasePt;
    AudioClip erase;

    int menuOption;

    AudioSource speaker;

    public AudioClip menuSwipe;
    public AudioClip transit;
    public AudioClip block;

    TMP_InputField roomNum;
    bool inserting;
    bool insertWarned;

    public TextAsset warningsTextEng;
    public TextAsset warningsTextPt;

    string[] warningsText;

    void Start()
    {
        speaker = GetComponent<AudioSource>();

        AudioSource.PlayClipAtPoint(transit, transform.position + new Vector3(0, 0, -10f));

        scene = SceneManager.GetActiveScene().buildIndex;

        menuOption = -1;

        if (GameManager.lang == 0)
        {
            menuOptions = menuOptionsEng;

            createRoomError = createRoomErrorEng;
            joinRoomError = joinRoomErrorEng;
            missionErrorTut = missionErrorTutEng;
            missionError = missionErrorEng;
            roleSelectionError =  roleSelectionErrorEng;

            warnInserting = warnInsertingEng;
            warnJoin = warnJoinEng;

            nums = numsEng;
            erase = eraseEng;

            warningsText = Regex.Split(warningsTextEng.text, "\n");
        } 
        else
        {
            menuOptions = menuOptionsPt;

            createRoomError = createRoomErrorPt;
            joinRoomError = joinRoomErrorPt;
            missionErrorTut = missionErrorTutPt;
            missionError = missionErrorPt;
            roleSelectionError = roleSelectionErrorPt;

            warnInserting = warnInsertingPt;
            warnJoin = warnJoinPt;

            nums = numsPt;
            erase = erasePt;

            warningsText = Regex.Split(warningsTextPt.text, "\n");
        }
        roomNum = GameObject.FindWithTag("RoomNum").GetComponent<TMP_InputField>();
    }

    void Update()
    {
        if (GameManager.narrator)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                roomNum.text = "";
                inserting = false;

                menuOption--;

                if (menuOption < 0)
                {
                    menuOption = 0;
                    AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(-0.01f, 0, -10f));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(-0.01f, 0, -10f));
                }
                Say();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                roomNum.text = "";
                inserting = false;

                menuOption++;

                if (menuOption > menuOptions.Length - 1)
                {
                    menuOption = menuOptions.Length - 1;
                    AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0.01f, 0, -10f));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0.01f, 0, -10f));
                }
                Say();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (menuOption >= 0)
                {
                    Do();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0.01f, 0, -10));
                    menuOption = Mathf.Min(menuOption + 1, 1);
                    Say();
                }
            }
            if (inserting)
            {
                CheckInserted();
            }
            if (!speaker.isPlaying)
            {
                warnings.text = "";
            }
        }
    }

    void Say()
    {
        speaker.clip = menuOptions[menuOption];
        speaker.Play();
    }

    void Do()
    {
        switch (menuOption)
        {
            case 0:
                GetComponent<MatchMakingManager>().Create();
                break;
            case 1:
                if (!inserting)
                {
                    WarnInserting();
                    inserting = true;
                }
                else
                {
                    GetComponent<MatchMakingManager>().Join();
                    inserting = false;
                    roomNum.text = "";
                }
                break;
            case 2:
                GetComponent<MatchMakingManager>().Back();
                break;
        }
    }

    void CheckInserted()
    {
        if (roomNum.text.Length > 3)
        {
            if (!insertWarned && !speaker.isPlaying)
            {
                WarnJoin();
                insertWarned = true;
            }
        }
        else
        {
            GetInsert();
        }
        GetErase();
    }

    void GetInsert()
    {
        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                roomNum.text += i.ToString();

                speaker.clip = nums[i];
                speaker.Play();
            }
        }
    }

    void GetErase()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            roomNum.text = roomNum.text.Remove(roomNum.text.Length - 1);

            insertWarned = false;

            speaker.clip = erase;
            speaker.Play();
        }
    }

    void WarnInserting()
    {
        speaker.clip = warnInserting;
        speaker.Play();
    }

    void WarnJoin()
    {
        speaker.clip = CombineClips(warnJoin, nums[Int32.Parse(roomNum.text[0].ToString())], nums[Int32.Parse(roomNum.text[1].ToString())], nums[Int32.Parse(roomNum.text[2].ToString())], nums[Int32.Parse(roomNum.text[3].ToString())]);
        speaker.Play();
    }

    public void CreateRoomError()
    {
        speaker.clip = createRoomError;
        speaker.Play();

        warnings.text = warningsText[0];
    }

    public void JoinRoomError()
    {
        speaker.clip = joinRoomError;
        speaker.Play();

        warnings.text = warningsText[1];
    }

    public void MissionError()
    {
        if (GameManager.tutorial)
        {
            speaker.clip = missionErrorTut;
            speaker.Play();


            warnings.text = warningsText[2];
        }
        else
        {
            speaker.clip = missionError;
            speaker.Play();


            warnings.text = warningsText[3];
        }
    }

    public void RoleSelectionError()
    {
        speaker.clip = roleSelectionError;
        speaker.Play();


        warnings.text = warningsText[4];
    }

    AudioClip CombineClips(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, 50000, false);
        result.SetData(data, 0);

        return result;
    }
}
