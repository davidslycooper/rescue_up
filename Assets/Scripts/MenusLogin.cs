using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class MenusLogin : MonoBehaviour
{
    int scene;

    [SerializeField] AudioClip[] menuOptionsEng;
    [SerializeField] AudioClip[] menuOptionsPt;

    AudioClip[] menuOptions;

    [SerializeField] AudioClip wrongIDErrorEng;
    [SerializeField] AudioClip wrongIDErrorPt;
    AudioClip wrongIDError;

    [SerializeField] TextMeshProUGUI warnings;

    //[SerializeField] AudioClip warnInsertingEng;
    //[SerializeField] AudioClip warnInsertingPt;
    //AudioClip warnInserting;

    [SerializeField] AudioClip warnLoginEng;
    [SerializeField] AudioClip warnLoginPt;
    AudioClip warnLogin;

    [SerializeField] AudioClip[] numsEng;
    [SerializeField] AudioClip[] numsPt;
    AudioClip[] nums;

    [SerializeField] AudioClip eraseEng;
    [SerializeField] AudioClip erasePt;
    AudioClip erase;

    int menuOption;

    AudioSource speaker;

    [SerializeField] AudioClip menuSwipe;
    [SerializeField] AudioClip transit;
    [SerializeField] AudioClip block;

    TMP_InputField id;
    bool inserting = true;
    bool insertWarned;

    string warningEng = "The ID inserted is incorrect. If you are unable to login, please contact the researchers.";
    string warningPt = "O ID introduzido está incorreto. Se não conseguir entrar, por favor contacte os investigadores.";

    string warning;

    void Start()
    {
        speaker = GetComponent<AudioSource>();

        AudioSource.PlayClipAtPoint(transit, transform.position + new Vector3(0, 0, -10f));

        scene = SceneManager.GetActiveScene().buildIndex;

        menuOption = 0;

        if (GameManager.lang == 0)
        {
            menuOptions = menuOptionsEng;

            wrongIDError = wrongIDErrorEng;

            //warnInserting = warnInsertingEng;
            warnLogin = warnLoginEng;

            nums = numsEng;
            erase = eraseEng;

            warning = warningEng;
        }
        else
        {
            menuOptions = menuOptionsPt;

            wrongIDError = wrongIDErrorPt;

            //warnInserting = warnInsertingPt;
            warnLogin = warnLoginPt;

            nums = numsPt;
            erase = erasePt;

            warning = warningPt;
        }
        id = GameObject.Find("ID").GetComponent<TMP_InputField>();

        if (GameManager.narrator)
        {
            Say();
        }
    }

    void Update()
    {
        if (GameManager.narrator)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                id.text = "";
                inserting = false;

                menuOption--;

                if (menuOption == 0)
                {
                    inserting = true;
                }
                else
                {
                    inserting = false;
                }

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
                id.text = "";

                menuOption++;

                if (menuOption == 0)
                {
                    inserting = true;
                }
                else
                {
                    inserting = false;
                }

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

        if (Input.GetMouseButton(0))
        {
            inserting = false;
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
                GetComponent<LoginMenuManager>().Login();
                id.text = "";
                break;
            case 1:
                GetComponent<LoginMenuManager>().Quit();
                break;
        }
    }

    void CheckInserted()
    {
        if (id.text.Length > 3)
        {
            if (!insertWarned && !speaker.isPlaying)
            {
                WarnLogin();
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
                id.text += i.ToString();

                speaker.clip = nums[i];
                speaker.Play();
            }
        }
    }

    void GetErase()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            id.text = id.text.Remove(id.text.Length - 1);

            insertWarned = false;

            speaker.clip = erase;
            speaker.Play();
        }
    }

    //void WarnInserting()
    //{
    //    speaker.clip = warnInserting;
    //    speaker.Play();
    //}

    void WarnLogin()
    {
        speaker.clip = CombineClips(warnLogin, nums[Int32.Parse(id.text[0].ToString())], nums[Int32.Parse(id.text[1].ToString())], nums[Int32.Parse(id.text[2].ToString())], nums[Int32.Parse(id.text[3].ToString())]);
        speaker.Play();
    }

    public void WarnIncorrectID()
    {
        speaker.clip = wrongIDError;
        speaker.Play();

        warnings.text = warning;
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
