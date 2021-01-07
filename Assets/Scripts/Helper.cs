using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Helper : MonoBehaviour
{
    PhotonView PV;

    AudioSource speaker;
    AudioSource transceiving;

    public AudioClip transceiverStart;
    public AudioClip transceiverEnd;

    public AudioClip[] helpAudioEng;
    public AudioClip[] helpAudioPt;

    public TextAsset helpEng;
    public TextAsset helpPt;

    AudioClip[] helpAudio;
    string[] help;

    TextMeshProUGUI subtitles;

    bool alreadyShut;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        speaker = GetComponents<AudioSource>()[0];
        transceiving = GetComponents<AudioSource>()[1];
        subtitles = GetComponentInChildren<TextMeshProUGUI>();

        alreadyShut = true;

        if (GameManager.lang == 0)
        {
            helpAudio = helpAudioEng;
            help = Regex.Split(helpEng.text, "\n");
        }
        else
        {
            helpAudio = helpAudioPt;
            help = Regex.Split(helpPt.text, "\n");
        }
    }

    void Update()
    {
        if (!alreadyShut && !speaker.isPlaying)
        {
            subtitles.text = "";
            transceiving.Stop();
            alreadyShut = true;
        }
    }

    public void ExplainUranium()
    {
        GameManager.uranium = true;
        GameManager.Save();

        speaker.clip = CombineClips(transceiverStart, helpAudio[0], transceiverEnd);
        speaker.Play();
        transceiving.Play();

        alreadyShut = false;

        subtitles.text = help[0].ToUpper();
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

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, 52100, false);
        result.SetData(data, 0);

        return result;
    }
}
