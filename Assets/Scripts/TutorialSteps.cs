using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Globalization;
using TMPro;

public class TutorialSteps : MonoBehaviour
{
    PhotonView PV;

    EntityManager entityMan;

    PlayerController pc;
    CraftingPanel cp;
    SubManager sm;
    ActiveSonar sonar;

    GameObject sub;

    AudioSource speaker;
    AudioSource transceiving;
    AudioSource ambient;

    public AudioClip die;
    public AudioClip win;

    public TextMeshProUGUI subtitles;
    public GameObject spaceWarn;

    public AudioClip transceiverStart;
    public AudioClip transceiverEnd;
    public AudioClip submerge;
    public AudioClip success;

    public TextAsset stepsSpawn;
    public TextAsset stepsCompletionPilot;
    public TextAsset stepsCompletionSonar;
    public TextAsset stepsControlPilot;
    public TextAsset stepsControlSonar;
    public TextAsset stepsAIPilot;
    public TextAsset stepsAISonar;

    public TextAsset stepsExplainEngPilot; // base
    public TextAsset stepsExplainEngSonar; // base
    public TextAsset stepsExplainPtPilot; // base
    public TextAsset stepsExplainPtSonar; // base

    public TextAsset stepsExplainEngSonarAlternate;
    public TextAsset stepsExplainPtSonarAlternate;

    public TextAsset stepsExplainEngPilotSingle;
    public TextAsset stepsExplainPtPilotSingle;

    public TextAsset stepsExplainEngSonarSingle;
    public TextAsset stepsExplainPtSonarSingle;

    public TextAsset stepsExplainEngSonarAlternateSingle;
    public TextAsset stepsExplainPtSonarAlternateSingle;


    string stepsExplain;
    string stepsCompletion;
    string stepsControl;
    string stepsAI;

    // base

    public AudioClip[] explainEngPilot1;
    public AudioClip[] explainEngPilot2;
    public AudioClip[] explainEngPilot3;
    public AudioClip[] explainEngPilot4;
    public AudioClip[] explainEngPilot5;
    public AudioClip[] explainEngPilot6;
    public AudioClip[] explainEngPilot7;
    public AudioClip[] explainEngPilot8;
    public AudioClip[] explainEngPilot9;
    public AudioClip[] explainEngPilot10;
    public AudioClip[] explainEngPilot11;
    public AudioClip[] explainEngPilot12;
    public AudioClip[] explainEngPilot13;
    public AudioClip[] explainEngPilot14;
    public AudioClip[] explainEngPilot15;
    public AudioClip[] explainEngPilot16;
    public AudioClip[] explainEngPilot17;
    public AudioClip[] explainEngPilot18;
    public AudioClip[] explainEngPilot19;

    public AudioClip[] explainEngSonar1;
    public AudioClip[] explainEngSonar2;
    public AudioClip[] explainEngSonar3;
    public AudioClip[] explainEngSonar4;
    public AudioClip[] explainEngSonar5;
    public AudioClip[] explainEngSonar6;
    public AudioClip[] explainEngSonar7;
    public AudioClip[] explainEngSonar8;
    public AudioClip[] explainEngSonar9;
    public AudioClip[] explainEngSonar10;
    public AudioClip[] explainEngSonar11;
    public AudioClip[] explainEngSonar12;
    public AudioClip[] explainEngSonar13;
    public AudioClip[] explainEngSonar14;
    public AudioClip[] explainEngSonar15;
    public AudioClip[] explainEngSonar16;
    public AudioClip[] explainEngSonar17;
    public AudioClip[] explainEngSonar18;
    public AudioClip[] explainEngSonar19;

    public AudioClip[] explainPtPilot1;
    public AudioClip[] explainPtPilot2;
    public AudioClip[] explainPtPilot3;
    public AudioClip[] explainPtPilot4;
    public AudioClip[] explainPtPilot5;
    public AudioClip[] explainPtPilot6;
    public AudioClip[] explainPtPilot7;
    public AudioClip[] explainPtPilot8;
    public AudioClip[] explainPtPilot9;
    public AudioClip[] explainPtPilot10;
    public AudioClip[] explainPtPilot11;
    public AudioClip[] explainPtPilot12;
    public AudioClip[] explainPtPilot13;
    public AudioClip[] explainPtPilot14;
    public AudioClip[] explainPtPilot15;
    public AudioClip[] explainPtPilot16;
    public AudioClip[] explainPtPilot17;
    public AudioClip[] explainPtPilot18;
    public AudioClip[] explainPtPilot19;

    public AudioClip[] explainPtSonar1;
    public AudioClip[] explainPtSonar2;
    public AudioClip[] explainPtSonar3;
    public AudioClip[] explainPtSonar4;
    public AudioClip[] explainPtSonar5;
    public AudioClip[] explainPtSonar6;
    public AudioClip[] explainPtSonar7;
    public AudioClip[] explainPtSonar8;
    public AudioClip[] explainPtSonar9;
    public AudioClip[] explainPtSonar10;
    public AudioClip[] explainPtSonar11;
    public AudioClip[] explainPtSonar12;
    public AudioClip[] explainPtSonar13;
    public AudioClip[] explainPtSonar14;
    public AudioClip[] explainPtSonar15;
    public AudioClip[] explainPtSonar16;
    public AudioClip[] explainPtSonar17;
    public AudioClip[] explainPtSonar18;
    public AudioClip[] explainPtSonar19;

    // selections

    public AudioClip[] explainEngSonarAlternate;
    public AudioClip[] explainPtSonarAlternate;

    public AudioClip[] explainEngPilotSingle;
    public AudioClip[] explainPtPilotSingle;

    public AudioClip[] explainEngSonarSingle;
    public AudioClip[] explainPtSonarSingle;

    List<AudioClip[]> explainAudio;

    public TextAsset warningsEng;
    public TextAsset warningsPt;

    string[] warnings;

    public AudioClip[] warningsAudioEng;
    public AudioClip[] warningsAudioPt;

    AudioClip[] warningsAudio;

    int curTask; 

    int explainStep;

    int curExplain;

    bool explaining;
    bool inTask;

    bool taskReady;
    bool[] partnerTaskReady;

    bool warnedTask;

    int partnerTask;

    bool understood;

    List<Step> steps;

    Step curStep;

    bool dead;
    bool rockHit;

    bool rotated;
    int pinged;

    float timeToWarn;
    bool alreadyShut;

    float silentTime;
    bool explained;
    bool repeating;

    public GameObject cueArrow;

    PlayerLog log;

    public void StartTutorial()
    {
        Config();
        NewStep();
    }

    void Config()
    {
        PV = GetComponent<PhotonView>();
        entityMan = GetComponent<EntityManager>();
        speaker = GetComponents<AudioSource>()[0];
        transceiving = GetComponents<AudioSource>()[1];
        ambient = GetComponents<AudioSource>()[2];

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        explainAudio = new List<AudioClip[]>();

        curTask = -1;
        curExplain = 0;

        if (GameManager.pilot)
        {
            stepsCompletion = stepsCompletionPilot.text;
            stepsControl = stepsControlPilot.text;
            stepsAI = stepsAIPilot.text;
            
            if (GameManager.lang == 0)
            {
                stepsExplain = stepsExplainEngPilot.text;
                explainAudio.Add(explainEngPilot1);
                explainAudio.Add(explainEngPilot2);
                explainAudio.Add(explainEngPilot3);
                explainAudio.Add(explainEngPilot4);
                explainAudio.Add(explainEngPilot5);
                explainAudio.Add(explainEngPilot6);
                explainAudio.Add(explainEngPilot7);
                explainAudio.Add(explainEngPilot8);
                explainAudio.Add(explainEngPilot9);
                explainAudio.Add(explainEngPilot10);
                explainAudio.Add(explainEngPilot11);
                explainAudio.Add(explainEngPilot12);
                explainAudio.Add(explainEngPilot13);
                explainAudio.Add(explainEngPilot14);
                explainAudio.Add(explainEngPilot15);
                explainAudio.Add(explainEngPilot16);
                explainAudio.Add(explainEngPilot17);
                explainAudio.Add(explainEngPilot18);
                explainAudio.Add(explainEngPilot19);

                if (!PhotonNetwork.InRoom)
                {
                    explainAudio[0][1] = explainEngPilotSingle[0];
                    explainAudio[0][2] = explainEngPilotSingle[1];
                    explainAudio[4][1] = explainEngPilotSingle[2];
                    explainAudio[5][0] = explainEngPilotSingle[3];
                    explainAudio[9][0] = explainEngPilotSingle[4];
                    explainAudio[9][1] = explainEngPilotSingle[5];
                    explainAudio[11][0] = explainEngPilotSingle[6];
                    explainAudio[13][0] = explainEngPilotSingle[7];
                    explainAudio[13][1] = explainEngPilotSingle[8];
                    explainAudio[15][0] = explainEngPilotSingle[9];
                    explainAudio[16][0] = explainEngPilotSingle[10];
                    explainAudio[17][0] = explainEngPilotSingle[11];
                }

                if (PhotonNetwork.InRoom)
                {
                    stepsExplain = stepsExplainEngPilot.text;
                }
                else
                {
                    stepsExplain = stepsExplainEngPilotSingle.text;
                }

                warningsAudio = warningsAudioEng;
                warnings = Regex.Split(warningsEng.text, "\n");
            }
            else
            {
                stepsExplain = stepsExplainPtPilot.text;
                explainAudio.Add(explainPtPilot1);
                explainAudio.Add(explainPtPilot2);
                explainAudio.Add(explainPtPilot3);
                explainAudio.Add(explainPtPilot4);
                explainAudio.Add(explainPtPilot5);
                explainAudio.Add(explainPtPilot6);
                explainAudio.Add(explainPtPilot7);
                explainAudio.Add(explainPtPilot8);
                explainAudio.Add(explainPtPilot9);
                explainAudio.Add(explainPtPilot10);
                explainAudio.Add(explainPtPilot11);
                explainAudio.Add(explainPtPilot12);
                explainAudio.Add(explainPtPilot13);
                explainAudio.Add(explainPtPilot14);
                explainAudio.Add(explainPtPilot15);
                explainAudio.Add(explainPtPilot16);
                explainAudio.Add(explainPtPilot17);
                explainAudio.Add(explainPtPilot18);
                explainAudio.Add(explainPtPilot19);

                if (!PhotonNetwork.InRoom)
                {
                    explainAudio[0][1] = explainPtPilotSingle[0];
                    explainAudio[0][2] = explainPtPilotSingle[1];
                    explainAudio[4][1] = explainPtPilotSingle[2];
                    explainAudio[5][0] = explainPtPilotSingle[3];
                    explainAudio[9][0] = explainPtPilotSingle[4];
                    explainAudio[9][1] = explainPtPilotSingle[5];
                    explainAudio[11][0] = explainPtPilotSingle[6];
                    explainAudio[13][0] = explainPtPilotSingle[7];
                    explainAudio[13][1] = explainPtPilotSingle[8];
                    explainAudio[15][0] = explainPtPilotSingle[9];
                    explainAudio[16][0] = explainPtPilotSingle[10];
                    explainAudio[17][0] = explainPtPilotSingle[11];
                }


                if (PhotonNetwork.InRoom)
                {
                    stepsExplain = stepsExplainPtPilot.text;
                }
                else
                {
                    stepsExplain = stepsExplainPtPilotSingle.text;
                }

                warningsAudio = warningsAudioPt;
                warnings = Regex.Split(warningsPt.text, "\n");
            }
        }
        else
        {
            stepsCompletion = stepsCompletionSonar.text;
            stepsControl = stepsControlSonar.text;
            stepsAI = stepsAISonar.text;

            if (GameManager.lang == 0)
            {
                explainAudio.Add(explainEngSonar1);
                explainAudio.Add(explainEngSonar2);
                explainAudio.Add(explainEngSonar3);
                explainAudio.Add(explainEngSonar4);
                explainAudio.Add(explainEngSonar5);
                explainAudio.Add(explainEngSonar6);
                explainAudio.Add(explainEngSonar7);
                explainAudio.Add(explainEngSonar8);
                explainAudio.Add(explainEngSonar9);
                explainAudio.Add(explainEngSonar10);
                explainAudio.Add(explainEngSonar11);
                explainAudio.Add(explainEngSonar12);
                explainAudio.Add(explainEngSonar13);
                explainAudio.Add(explainEngSonar14);
                explainAudio.Add(explainEngSonar15);
                explainAudio.Add(explainEngSonar16);
                explainAudio.Add(explainEngSonar17);
                explainAudio.Add(explainEngSonar18);
                explainAudio.Add(explainEngSonar19);

                if (GameManager.alternate)
                {
                    explainAudio[1][0] = explainEngSonarAlternate[0];
                    explainAudio[1][1] = explainEngSonarAlternate[1];
                    explainAudio[2][2] = explainEngSonarAlternate[2];
                    explainAudio[3][2] = explainEngSonarAlternate[3];
                    explainAudio[7][3] = explainEngSonarAlternate[4];
                    explainAudio[8][2] = explainEngSonarAlternate[5];
                    explainAudio[10][1] = explainEngSonarAlternate[6];
                }
                if (!PhotonNetwork.InRoom)
                {
                    explainAudio[0][1] = explainEngSonarSingle[0];
                    explainAudio[0][2] = explainEngSonarSingle[1];
                    explainAudio[4][1] = explainEngSonarSingle[12];
                    explainAudio[5][0] = explainEngSonarSingle[2];
                    explainAudio[6][0] = explainEngSonarSingle[3];
                    explainAudio[9][1] = explainEngSonarSingle[4];
                    explainAudio[11][0] = explainEngSonarSingle[5];
                    explainAudio[13][1] = explainEngSonarSingle[6];
                    explainAudio[15][0] = explainEngSonarSingle[7];
                    explainAudio[16][0] = explainEngSonarSingle[8];
                    explainAudio[17] = new AudioClip[] { explainEngSonarSingle[9], explainEngSonarSingle[10], explainEngSonarSingle[11] };
                }

                if (GameManager.alternate)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        stepsExplain = stepsExplainEngSonarAlternate.text;
                    }
                    else
                    {
                        stepsExplain = stepsExplainEngSonarAlternateSingle.text;
                    }
                }
                else
                {
                    if (PhotonNetwork.InRoom)
                    {
                        stepsExplain = stepsExplainEngSonar.text;
                    }
                    else
                    {
                        stepsExplain = stepsExplainEngSonarSingle.text;
                    }
                }

                warningsAudio = warningsAudioEng;
                warnings = Regex.Split(warningsEng.text, "\n");
            }
            else
            {
                explainAudio.Add(explainPtSonar1);
                explainAudio.Add(explainPtSonar2);
                explainAudio.Add(explainPtSonar3);
                explainAudio.Add(explainPtSonar4);
                explainAudio.Add(explainPtSonar5);
                explainAudio.Add(explainPtSonar6);
                explainAudio.Add(explainPtSonar7);
                explainAudio.Add(explainPtSonar8);
                explainAudio.Add(explainPtSonar9);
                explainAudio.Add(explainPtSonar10);
                explainAudio.Add(explainPtSonar11);
                explainAudio.Add(explainPtSonar12);
                explainAudio.Add(explainPtSonar13);
                explainAudio.Add(explainPtSonar14);
                explainAudio.Add(explainPtSonar15);
                explainAudio.Add(explainPtSonar16);
                explainAudio.Add(explainPtSonar17);
                explainAudio.Add(explainPtSonar18);
                explainAudio.Add(explainPtSonar19);

                if (GameManager.alternate)
                {
                    explainAudio[1][0] = explainPtSonarAlternate[0];
                    explainAudio[1][1] = explainPtSonarAlternate[1];
                    explainAudio[2][2] = explainPtSonarAlternate[2];
                    explainAudio[3][2] = explainPtSonarAlternate[3];
                    explainAudio[7][3] = explainPtSonarAlternate[4];
                    explainAudio[8][2] = explainPtSonarAlternate[5];
                    explainAudio[10][1] = explainPtSonarAlternate[6];
                } 
                if (!PhotonNetwork.InRoom)
                {
                    explainAudio[0][1] = explainPtSonarSingle[0];
                    explainAudio[0][2] = explainPtSonarSingle[1];
                    explainAudio[4][1] = explainPtSonarSingle[12];
                    explainAudio[5][0] = explainPtSonarSingle[2];
                    explainAudio[6][0] = explainPtSonarSingle[3];
                    explainAudio[9][1] = explainPtSonarSingle[4];
                    explainAudio[11][0] = explainPtSonarSingle[5];
                    explainAudio[13][1] = explainPtSonarSingle[6];
                    explainAudio[15][0] = explainPtSonarSingle[7];
                    explainAudio[16][0] = explainPtSonarSingle[8];
                    explainAudio[17] = new AudioClip[] { explainPtSonarSingle[9], explainPtSonarSingle[10], explainPtSonarSingle[11] };
                }

                if (GameManager.alternate)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        stepsExplain = stepsExplainPtSonarAlternate.text;
                    } 
                    else
                    {
                        stepsExplain = stepsExplainPtSonarAlternateSingle.text;
                    }
                } 
                else
                {
                    if (PhotonNetwork.InRoom)
                    {
                        stepsExplain = stepsExplainPtSonar.text;
                    }
                    else
                    {
                        stepsExplain = stepsExplainPtSonarSingle.text;
                    }
                }

                warningsAudio = warningsAudioPt;
                warnings = Regex.Split(warningsPt.text, "\n");
            }            
            cp = GetComponent<PhotonPlayer>().GetPanel().GetComponent<CraftingPanel>();
            sonar = GetComponent<PhotonPlayer>().GetPanel().GetComponentInChildren<ActiveSonar>();
        }
        sub = GetComponent<PhotonPlayer>().GetSub();
        pc = sub.GetComponent<PlayerController>();
        sm = sub.GetComponent<SubManager>();

        partnerTaskReady = new bool[18];

        ConfigSteps();
    }

    void Update()
    {
        if (!PauseMenu.paused)
        {
            if (explaining)
            {
                CheckExplaining();

            }
            else
            {
                CheckStepCompletion();
                CheckLoaded();
            }

            if (!dead && !taskReady)
            {
                CheckRepeatExplain();
                if (!GameManager.pilot && !explaining && !taskReady)
                {
                    GetInput();
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!PauseMenu.paused)
        {
            if (GameManager.pilot && !explaining && !taskReady && !dead)
            {
                GetInput();
            }
        }
    }

    void ConfigSteps()
    {
        steps = new List<Step>();

        for (int i = 0; i < 19; i++)
        {
            steps.Add(new Step(i));
        }
        ConfigStepSpawnRules();
        ConfigStepExplain();
        ConfigStepCompletion();
        ConfigControlAllowances();
        ConfigAISteps();
    }

    void ConfigStepSpawnRules()
    {
        string txt = stepsSpawn.text;
        string[] lines = Regex.Split(txt, "\n");

        for (int i = 0; i < 19; i++)
        {
            if (lines[i].Trim() != "null")
            {
                string[] spawnRule = Regex.Split(lines[i], ":");
                float[] spawnPos = Array.ConvertAll(Regex.Split(spawnRule[1], "/"), s => float.Parse(s, CultureInfo.InvariantCulture.NumberFormat));

                steps[i].SetSpawnRules(spawnRule[0].Trim(), spawnPos);
            }
        }
    }

    void ConfigStepExplain()
    {
        string txt = stepsExplain;
        string[] lines = Regex.Split(txt, "\n");

        for (int i = 0; i < 19; i++)
        {
            List<string> explains = Regex.Split(lines[i], "/").ToList();

            steps[i].SetExplain(explains);
        }
    }

    void ConfigStepCompletion()
    {
        string txt = stepsCompletion;
        string[] lines = Regex.Split(txt, "\n");

        for (int i = 0; i < 19; i++)
        {
            string[] completion = Regex.Split(lines[i], "=");

            string[] rules = Regex.Split(completion[0], ":");

            steps[i].SetCompletion(rules[0], rules[1], completion.Length > 1, rules[2]);
        }
    }

    void ConfigControlAllowances()
    {
        string txt = stepsControl;
        string[] lines = Regex.Split(txt, "\n");

        for (int i = 0; i < 19; i++)
        {
            if (lines[i].Trim() != "null")
            {
                string[] split = Regex.Split(lines[i], ":");

                string[] controls = Regex.Split(split[0], ",");
                int panels = Int32.Parse(split[1]);
                string[] prevent = (split.Length > 2) ? Regex.Split(split[2], ",") : new string[0];

                steps[i].SetControl(controls, panels, prevent);
            }
        }
    }

    void ConfigAISteps()
    {
        string txt = stepsAI;
        string[] lines = Regex.Split(txt, "\n");

        for (int i = 0; i < 19; i++)
        {
            string[] ai = Regex.Split(lines[i], "=");
            string[] rules = Regex.Split(ai[0], ":");

            steps[i].SetAI(rules[0], rules[1], ai.Length > 1);
        }
    }

    void Explain()
    {
        Say(explainAudio[curTask][curExplain], curStep.explain[curExplain]);

        spaceWarn.SetActive(false);
    }

    void GetInput()
    {
        if (curStep.controls != null)
        {
            foreach (string control in curStep.controls)
            {
                MethodInfo mi = this.GetType().GetMethod("CONTROL_" + control.Trim());

                mi.Invoke(this, null);
            }

            if (!speaker.isPlaying) {

                foreach (string prevent in curStep.preventControls)
                {
                    MethodInfo mi = this.GetType().GetMethod("PREVENT_" + prevent.Trim());

                    if ((bool) mi.Invoke(this, null))
                    {
                        WarnControl();
                    }
                }
            }
        }
    }

    void CheckStepCompletion()
    {
        if (!speaker.isPlaying && !inTask)
        {
            if (curStep.spawnRules != null && curTask < 17)
            {
                Spawn();
            }
            if (curStep.completion != "COMPLETION_End")
            {
                Ambient(1);
                GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 0f);
                Shut(true);
                spaceWarn.SetActive(false);
            }
            else
            {
                Shut(false);
                spaceWarn.SetActive(true);
            }

            if (!PhotonNetwork.InRoom && !curStep.aiDelay)
            {
                MethodInfo miai = this.GetType().GetMethod(curStep.ai);
                miai.Invoke(this, new string[] { curStep.aiParam });
            }

            inTask = true;
            log.EndStep(curTask);
        }

        if (inTask && !taskReady)
        {
            if (!speaker.isPlaying && !alreadyShut)
            {
                Shut(true);
            }
            MethodInfo mi = this.GetType().GetMethod(curStep.completion);

            if ((bool)mi.Invoke(this, new string[] { curStep.completionParam }))
            {
                if (PhotonNetwork.InRoom && curTask < partnerTaskReady.Length)
                {
                    PV.RPC("RPC_TaskReady", RpcTarget.Others, curTask);
                }
                taskReady = true;

                if (!curStep.completionWait || partnerTaskReady[curTask] || !PhotonNetwork.InRoom)
                {
                    if (!PhotonNetwork.InRoom && curStep.aiDelay)
                    {
                        MethodInfo miai = this.GetType().GetMethod(curStep.ai);
                        miai.Invoke(this, new string[] { curStep.aiParam });
                    }
                    EndStep();
                }
                else if (curTask != 0)
                {
                    GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 0.5f);
                    spaceWarn.SetActive(false);
                }
                timeToWarn = Time.time + 1f;
            }
            else
            {
                Confirm();
                CheckNoAction();
            }
        }
        else
        {
            if (!speaker.isPlaying && !alreadyShut)
            {
                Shut(true);
            }
            if (!curStep.completionWait || partnerTaskReady[curTask] || !PhotonNetwork.InRoom)
            {
                EndStep();
            }
            else if ((!warnedTask && timeToWarn < Time.time) || (!speaker.isPlaying && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))))
            {
                WarnWaitingForPartnerTask();
                warnedTask = true;
            }
        }
    }

    void CheckExplaining()
    {
        if (!speaker.isPlaying && !alreadyShut)
        {
            Shut(false);
            spaceWarn.SetActive(true);
            explained = true;
        }

        if (explained)
        {
            if (curExplain >= curStep.explain.Count - 1)
            {                
                explaining = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shut(true);
                curExplain++;
                if (curExplain < curStep.explain.Count)
                {
                    explaining = true;
                    explained = false;
                    Explain();
                }
            }
            CheckNoAction();
        } 
    }

    void CheckNoAction()
    {
        if (!speaker.isPlaying && !dead)
        {
            if (explaining)
            {
                if (Time.time - silentTime > 8f)
                {
                    WarnPressSpace();
                }
            }
            else if (Time.time - silentTime > curStep.completionTime)
            {
                if (curStep.completion == "COMPLETION_End" && !taskReady)
                {
                    WarnPressSpace();
                }
                else if (!taskReady)
                {
                    WarnAlt(); //Não sabes o que fazer?
                }
            }
        }
    }

    void Confirm()
    {
        if (!understood && Input.GetKey(KeyCode.Space))
        {
            understood = true;
        }
        if (!rotated && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            rotated = true;
        }
    }

    void CheckRepeatExplain()
    {
        if ((explaining || inTask) && (Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.LeftAlt)))
        {
            repeating = true;
            Shut(true);
            Explain();
        }
        if (repeating && inTask && Input.GetKeyDown(KeyCode.Space))
        {
            Shut(true);
        }
    }

    void CheckLoaded()
    {
        if (GameManager.pilot && curTask == 15 && !rockHit && (!sm.botLoaded || !sm.topLoaded) && GameObject.FindWithTag("Torpedo") == null)
        {
            sm.botLoaded = true;
            sm.topLoaded = true;
            WarnReloaded();
        }
    }

    public void Spawn()
    {
        List<float[]> toSpawn = new List<float[]>();

        float[] spawnRules = curStep.spawnRules;

        if ((int) spawnRules[0] < 4)
        {
            for (int i = 0; i < (spawnRules.Length - 1) / 2; i++)
            {
                toSpawn.Add(new float[] { spawnRules[i * 2 + 1], spawnRules[i * 2 + 2], spawnRules[0] });
            }
            entityMan.SpawnMinerals(toSpawn);
        }
        else if ((int) spawnRules[0] == 4)
        {
            toSpawn.Add(new float[] { spawnRules[1], spawnRules[2] });

            entityMan.SpawnBrokenRocks(toSpawn);
        }
        else if ((int) spawnRules[0] == 5)
        {
            toSpawn.Add(new float[] { spawnRules[1], spawnRules[2] });

            entityMan.SpawnTreasure(toSpawn);
        }
        else if ((int) spawnRules[0] == 6 && (GameManager.pilot || !PhotonNetwork.InRoom))
        {
            toSpawn.Add(new float[] { spawnRules[1], spawnRules[2], 0 });

            entityMan.SpawnEnemies(toSpawn);
        }
    }

    void WarnWaitingForPartnerTask()
    {
        Say(warningsAudio[0], warnings[0]);
    }

    void WarnReloaded()
    {
        Say(warningsAudio[1], warnings[1]);
    }

    void WarnPressSpace()
    {
        Say(warningsAudio[2], warnings[2]);
    }

    void WarnAlt()
    {
        Say(warningsAudio[3], warnings[3]);
    }

    void WarnControl()
    {
        Say(warningsAudio[4], warnings[4]);
    }

    void Say(AudioClip saying, string text)
    {
        speaker.clip = CombineClips(transceiverStart, saying, transceiverEnd);
        speaker.Play();
        transceiving.Play();

        subtitles.text = text.ToUpper();

        alreadyShut = false;
    }

    void Shut(bool hideText)
    {
        speaker.Stop();
        transceiving.Stop();

        if (hideText)
        {
            subtitles.text = "";
        }

        silentTime = Time.time;
        alreadyShut = true;
        repeating = false;
    }

    void Ambient(int which)
    {
        ambient.Stop();

        switch (which)
        {
            case 0:
                ambient.volume = 0.05f;
                break;
            default:
                ambient.volume = 0.2f;
                break;
        }
        ambient.Play();
    }

    void EndStep()
    {
        if (curStep.completion != "COMPLETION_End")
        {
            AudioSource.PlayClipAtPoint(success, sub.transform.position + new Vector3(0, 5, 0));
        }

        if (curTask == 0)
        {
            Submerge();
        }
        curExplain = 0;
        inTask = false;
        understood = false;
        taskReady = false;
        warnedTask = false;
        explained = false;
        spaceWarn.SetActive(false);

        cueArrow.SetActive(false);

        if (curTask > 17)
        {
            GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 1f);
        } 
        else
        { 
            GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 10f, 0.8f);
        }

        if (curTask > 17)
        {
            End();
        }
        else
        {
            NewStep();
        }
    }

    void NewStep()
    {
        curTask++;
        curStep = steps[curTask]; // out of range?
        if (curStep.spawnRules != null && curTask > 16)
        {
            Spawn();
        }
        Ambient(0);
        explaining = true;

        log.NewStep(curTask);

        Explain();
    }

    void End()
    {
        log.EndTutorial();

        if (PhotonNetwork.InRoom)
        {
            GameManager.roomNum = "";
            PhotonNetwork.LeaveRoom();
        }

        Cursor.visible = true;
        SceneManager.LoadScene(GameManager.loadingScene);
    }

    public void Pause()
    {
        if (!alreadyShut)
        {
            speaker.Pause();
            transceiving.Pause();
        }
        ambient.Pause();
    }

    public void Unpause()
    {
        if (!alreadyShut)
        {
            if (explaining)
            {
                repeating = true;
                Shut(true);
                Explain();
            }
            else
            {
                speaker.UnPause();
                transceiving.UnPause();
            }
        }
        ambient.UnPause();
    }

    void Submerge()
    {
        AudioSource.PlayClipAtPoint(submerge, sub.transform.position + new Vector3(0, 5, 0));

        //GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 0.5f);
    }

    public void Die()
    {
        GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 1f);

        speaker.clip = die;
        speaker.Play();

        dead = true;
    }

    public void Win()
    {
        GameObject.FindWithTag("BlackScreen").GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 1f);

        speaker.clip = win;
        speaker.Play();

        dead = true;
    }

    public void RockHit()
    {
        rockHit = true;
    }

    // Completion Checks

    public bool COMPLETION_True(string nothing)
    {
        return !explaining;
    }

    public bool COMPLETION_End(string nothing)
    {
        return !explaining && understood;
    }

    public bool COMPLETION_Pos(string pos)
    {
        float[] xy = Regex.Split(pos, "/").Select(x => float.Parse(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
        Vector3 position = new Vector3(xy[0], xy[1]);

        return Vector3.Distance(sub.transform.position, position) < xy[2];
    }

    public bool COMPLETION_State(string state)
    {
        return sm.state == Int32.Parse(state);
    }

    public bool COMPLETION_Ping(string nothing)
    {
        return pinged > 4 && ((!GameManager.alternate) ? rotated : true);
    }

    public bool COMPLETION_Caught(string qty)
    {
        return entityMan.caught >= Int32.Parse(qty) && ((cp != null) ? !cp.speaker.isPlaying : true);
    }

    public bool COMPLETION_Detected(string nothing)
    {
        return sonar.detectedInd < 6;
    }

    public bool COMPLETION_Panel(string nothing)
    {
        return cp.menuState == 1 && !cp.speaker.isPlaying;
    }

    public bool COMPLETION_Battery(string nothing)
    {
        return cp.batteryLevel > 9 && !cp.speaker.isPlaying;
    }

    public bool COMPLETION_Sonar(string nothing)
    {
        return sonar.updates > 0 && !cp.speaker.isPlaying;
    }

    public bool COMPLETION_SonarInfo(string nothing)
    {
        return sonar.sonarInfo && !sonar.speaker.isPlaying;
    }

    public bool COMPLETION_Flare(string nothing)
    {
        return sm.flareLoaded && ((cp != null) ? !cp.speaker.isPlaying : true);
    }

    public bool COMPLETION_Torpedo(string nothing)
    {
        return sm.topLoaded && sm.botLoaded && ((cp != null) ? !cp.speaker.isPlaying : true);
    }

    public bool COMPLETION_Treasure(string pos)
    {
        float[] xy = Regex.Split(pos, "/").Select(x => float.Parse(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
        Vector3 position = new Vector3(xy[0], xy[1]);

        return Vector3.Distance(sub.transform.position, position) < xy[2] && entityMan.caught >= 6 && ((cp != null) ? !cp.speaker.isPlaying : true);
    }

    public bool COMPLETION_Dead(string nothing)
    {
        return dead && !speaker.isPlaying;
    }


    // Control Allowances

    public void CONTROL_Move()
    {
        pc.Move();
    }

    public void CONTROL_Manage()
    {
        if (pc.Manage()) Shut(true);
    }

    public void CONTROL_ManageItems() 
    {
        pc.ManageItems();
    }

    public void CONTROL_Use()
    {
        if (pc.Use()) Shut(true);
    }

    public void CONTROL_Hand()
    {
        if (pc.Hand()) Shut(true);
    }

    public void CONTROL_Signer()
    {
        if (pc.Signer()) Shut(true);
    }

    public void CONTROL_Cannons()
    {
        if (pc.Cannons()) Shut(true);
    }

    public void CONTROL_Saver()
    {
        if (pc.Saver()) Shut(true);
    }

    ///

    public void CONTROL_Sonar()
    {
        if (sonar.Sonar()) 
        { 
            Shut(true);
            pinged++;
        }
    }

    public void CONTROL_SonarInfo()
    {
        if (sonar.SonarInfo()) Shut(true);
    }

    public void CONTROL_SonarManage()
    {
        sonar.SonarManage();
    }

    public void CONTROL_PanelNav()
    {
        if (cp.PanelNav(curStep.panels)) Shut(true);
    }

    public void CONTROL_Action()
    {
        if (cp.Action()) Shut(true);
    }

    public void CONTROL_Storage()
    {
        if (cp.Storage()) Shut(true);
    }

    // Control Prevent

    public bool PREVENT_Move()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) > 0;
    }

    public bool PREVENT_Manage()
    {
        return Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4);
    }

    public bool PREVENT_Use()
    {
        return Input.GetMouseButtonDown(0);
    }

    //

    public bool PREVENT_Sonar()
    {
        return (!GameManager.alternate && Input.GetMouseButtonDown(0)) || (GameManager.alternate && Input.GetKeyDown(KeyCode.Space));
    }

    public bool PREVENT_SonarInfo()
    {
        return (!GameManager.alternate && Input.GetMouseButtonDown(1)) || (GameManager.alternate && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)));
    }

    public bool PREVENT_PanelNav()
    {
        return (!GameManager.alternate && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))) || 
            (GameManager.alternate && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)));
    }

    public bool PREVENT_Action()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    // AI Steps

    public void AI_End(string nothing)
    {
        partnerTaskReady[curTask] = true;
    }

    public void AI_State(string state)
    {
        if (state == "2")
        {
            sub.transform.position = new Vector3(-0.3f, -8.4f);
        }
        sm.ChangeState(Int32.Parse(state));
    }

    public void AI_Pos(string pos)
    {
        float[] xy = Regex.Split(pos, "/").Select(x => float.Parse(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
        Vector3 position = new Vector3(xy[0], xy[1]);

        sub.transform.position = position;
    }

    public void AI_Catch(string mineral)
    {
        int[] qt = Regex.Split(mineral, "/").Select(x => Int32.Parse(x)).ToArray();

        entityMan.caught += qt[0];
        sm.SimulateCatch(qt[0], qt[1]);
    }

    public void AI_Cue(string pos)
    {
        float[] xy = Regex.Split(pos, "/").Select(x => float.Parse(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
        Vector3 position = new Vector3(xy[0], xy[1]);

        cueArrow.SetActive(true);
        cueArrow.GetComponent<CueArrow>().SetPos(position);
    }

    public void AI_Battery(string nothing)
    {
        sm.ChargeBattery(1);
    }

    public void AI_Flare(string nothing)
    {
        sm.LoadSigner();
    }

    public void AI_Torpedo(string nothing)
    {
        sm.LoadCannon(true);
        sm.LoadCannon(false);
    }

    public void AI_Fire(string nothing)
    {
        sm.SimulateFire();
    }

    public void AI_Treasure(string nothing)
    {
        entityMan.caught ++;
        entityMan.DestroyTreasure();
        sm.SimulateCatch(1, 4);

        curStep.completionParam = "-5/-9/2";

        curStep.aiDelay = true;
        curStep.ai = "AI_Pos";
        curStep.aiParam = "2.4/0";
    }

    public void AI_Die(string nothing)
    {
    }

    [PunRPC]
    public void RPC_TaskReady(int task)
    {
        partnerTaskReady[task] = true;
    }

    class Step
    {
        public int num;
        public float[] spawnRules;
        public List<string> explain;

        public string completion;
        public string completionParam;
        public bool completionWait;
        public float completionTime;

        public string[] controls;
        public string[] preventControls;

        public string ai;
        public string aiParam;
        public bool aiDelay;

        public int panels;

        public Step(int ind)
        {
            num = ind;
            spawnRules = null;
            explain = new List<string>();
            completion = "";
            completionParam = "";
            completionWait = false;
            completionTime = 50;
            controls = null;
            preventControls = null;
            ai = "";
            aiParam = "";
            aiDelay = false;
            panels = 0;
        }

        public void SetSpawnRules(string what, params float[] pos)
        {
            spawnRules = new float[1 + pos.Length];

            switch(what) // 0: coal; 1: uranium; 2: quartz; 3: amber; 4: broken; 5: treasure; 6: enemy
            {
                case "coal":
                    spawnRules[0] = 0;
                    break;
                case "uranium":
                    spawnRules[0] = 1;
                    break;
                case "quartz":
                    spawnRules[0] = 2;
                    break;
                case "amber":
                    spawnRules[0] = 3;
                    break;
                case "broken":
                    spawnRules[0] = 4;
                    break;
                case "treasure":
                    spawnRules[0] = 5;
                    break;
                case "enemy":
                    spawnRules[0] = 6;
                    break;  
            }

            for(int i = 0; i < pos.Length; i++)
            {
                spawnRules[i + 1] = pos[i];
            }
        }

        public void SetExplain(List<string> explains)
        {
            explain = explains;
        }

        public void SetCompletion(string type, string param, bool wait, string time)
        {
            completion = "COMPLETION_" + type;
            completionParam = param;
            completionWait = wait;
            completionTime = float.Parse(time, CultureInfo.InvariantCulture.NumberFormat);
        }

        public void SetControl(string[] controlAll, int panelNum, string[] prevent)
        {
            panels = panelNum;
            controls = controlAll;
            preventControls = prevent;
        }

        public void SetAI(string type, string param, bool delay)
        {
            ai = "AI_" + type;
            aiParam = param;
            aiDelay = delay;
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

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, 52100, false); //52100
        result.SetData(data, 0);

        return result;
    }
}
