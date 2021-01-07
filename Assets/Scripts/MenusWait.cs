using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class MenusWait : MonoBehaviour
{
    int scene;
    public AudioClip[] menuOptionsEng;
    public AudioClip[] menuOptionsPt;

    AudioClip[] menuOptions;

    public AudioClip[] numsEng;
    public AudioClip[] numsPt;
    AudioClip[] nums;

    int menuOption;

    AudioSource speaker;

    public AudioClip menuSwipe;
    public AudioClip transit;
    public AudioClip block;

    void Start()
    {
        AudioSource.PlayClipAtPoint(transit, transform.position + new Vector3(0, 0f, -10f));

        scene = SceneManager.GetActiveScene().buildIndex;

        menuOption = -1;

        menuOptions = (GameManager.lang == 0)? menuOptionsEng: menuOptionsPt;
        nums = (GameManager.lang == 0) ? numsEng : numsPt;

        speaker = GetComponent<AudioSource>();

        if (GameManager.narrator)
        {
            speaker.clip = CombineClips(menuOptions[0], nums[Int32.Parse(GameManager.roomNum[0].ToString())], nums[Int32.Parse(GameManager.roomNum[1].ToString())], nums[Int32.Parse(GameManager.roomNum[2].ToString())], nums[Int32.Parse(GameManager.roomNum[3].ToString())]);
            speaker.Play();

            menuOption = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
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
    }

    void Say()
    {
        if (menuOption == 0)
        {
            speaker.clip = CombineClips(menuOptions[menuOption], nums[Int32.Parse(GameManager.roomNum[0].ToString())], nums[Int32.Parse(GameManager.roomNum[1].ToString())], nums[Int32.Parse(GameManager.roomNum[2].ToString())], nums[Int32.Parse(GameManager.roomNum[3].ToString())]);
        } else
        {
            speaker.clip = menuOptions[menuOption];
        }
        speaker.Play();
    }

    void Do()
    {
        switch (menuOption)
        {
            case 0:
                Say();
                break;
            case 1:
                GetComponent<WaitRoomManager>().Cancel();
                break;
        }
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

        int pitch = (GameManager.lang == 0) ? 45000 : 40000;

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, pitch, false);
        result.SetData(data, 0);

        return result;
    }
}