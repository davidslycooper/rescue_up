using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSonar : MonoBehaviour
{

    bool tutorial;

    float z;

    [Range(1f, 100f)]
    public float rotTick;

    public int detectedInd;
    Vector3 detectedPos;

    public GameObject beepPrefab;
    public GameObject beep;

    [Range(0.01f, 0.5f)]
    public float beepSpeed;

    public AudioSource speaker;
    AudioSource menuSpeaker;

    public AudioClip[] detectedSpeakEng;
    public AudioClip[] cardinalSpeakEng;

    public AudioClip[] detectedSpeakPt;
    public AudioClip[] cardinalSpeakPt;

    AudioClip[] detectedSpeak;
    AudioClip[] cardinalSpeak;
    public AudioClip tick;

    float tickPitch;

    [Range(1f, 5f)]
    public float delay;

    bool readyToPing;

    float pingTime;

    public bool sonarInfo;
    public int updates;

    int facing;
    int trueFacing;

    PlayerLog log;

    void Start()
    {
        speaker = GetComponent<AudioSource>();
        menuSpeaker = GetComponentInParent<AudioSource>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        detectedSpeak = (GameManager.lang == 0) ? detectedSpeakEng : detectedSpeakPt;
        cardinalSpeak = (GameManager.lang == 0) ? cardinalSpeakEng : cardinalSpeakPt;

        detectedInd = 6;

        pingTime = Time.time;
    }

    void Update()
    {
        if (!GameManager.tutorial)
        {
            Sonar();
            SonarInfo();
            SonarManage();
        }
    }

    public void SonarManage()
    {
        if (Time.time > pingTime + delay)
        {
            readyToPing = true;

            if (beep != null)
            {
                Reset();
            }
        }
    }

    public bool Sonar()
    {
        if (!PauseMenu.paused)
        {
            if (!GameManager.alternate)
            {
                DirectionMouse();

                if (readyToPing && Input.GetMouseButtonDown(0))
                {
                    if (beep != null)
                    {
                        Reset();
                    }
                    Ping();
                    return true;
                }
            }
            else
            {
                if (readyToPing && Input.GetKeyDown(KeyCode.Space) && DirectionArrows())
                {
                    if (beep != null)
                    {
                        Reset();
                    }
                    Ping();
                    return true;
                }
            }
        }
        return false;
    }

    public bool SonarInfo()
    {
        if (!PauseMenu.paused)
        {
            if (!GameManager.alternate)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Say();
                    sonarInfo = true;
                    return true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    Say();
                    sonarInfo = true;
                    return true;
                }
            }
        }
        return false;
    }

    bool DirectionArrows()
    {
        int angle = 0;
        int arrows = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            angle += 90;
            arrows++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle += 180;
            arrows++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            angle += 270;
            arrows++;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            angle += (angle >= 270)? 360: 0;
            arrows++;
        }

        if (arrows > 0)
        {
            z = angle / arrows;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        }

        return arrows > 0 && arrows < 3;
    }

    void DirectionMouse()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (mouseWheel != 0)
        {
            menuSpeaker.Stop(); // Deixo?

            if (z > 270 && z < 360 || z < 90 && z >= 0)
            {
                tickPitch = (((540f - z) % 360f) - 90f) * 20f + 1000f;
            }
            else
            {
                tickPitch = (z - 90f) * 20f + 1000f;
            }

            Tick(transform.position + transform.right * 0.5f);

            z = ((z + mouseWheel * rotTick) + 360) % 360;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));

            //int newDirection = (int) z / 45;

            //if (newDirection != facing)
            //{
            //    trueFacing = ((newDirection > facing && (facing != 0 || newDirection != 7)) || (facing == 7 && newDirection == 0)) ? newDirection : (newDirection + 1) % 8;
            //    menuSpeaker.clip = cardinalSpeak[trueFacing];
            //    facing = newDirection;
            //}
        }
    }

    void Reset()
    {
        Destroy(beep);
    }

    public void Caught(int mineralType)
    {
        if (detectedInd == mineralType) // UM POUCO ALDRABADO, MAS POR AGORA SOLUCIONA
        {
            detectedInd = 6;
        }
    }

    void Ping()
    {
        if (!GameManager.tutorial)
        {
            log.Pulse(Time.time - pingTime, transform.position, z);
        }

        detectedInd = 6;

        menuSpeaker.Stop(); // Deixo?

        beep = Instantiate(beepPrefab, transform.position + transform.right * 0.2f, Quaternion.Euler(new Vector3(0, 0, z)));
        beep.GetComponent<Beep>().SetSpeed(beepSpeed);
        pingTime = Time.time;
    }

    public void Detect(int type, Vector3 pos, int id)
    {
        detectedInd = type;
        detectedPos = pos;

        if (!GameManager.tutorial)
        {
            log.Detected(type, id, transform.position, pos);
        }
    }

    void Say()
    {
        if (!GameManager.tutorial)
        {
            log.Info();
        }

        int cardinal = -1;
        float angle = (180 / Mathf.PI) * Mathf.Atan2(transform.position.y - detectedPos.y, transform.position.x - detectedPos.x); // 0 = Oeste; 1 = Sudoeste; 2 = Sul; 3 = Sudeste; 4 = Este; 5 = Nordeste; 6 = Norte; 7 = Noroeste

        if (Mathf.Abs(angle) < 20)
        {
            cardinal = 0;
        } else if (angle >= 20 && angle <= 70)
        {
            cardinal = 1;
        } else if (angle > 70 && angle < 110)
        {
            cardinal = 2;
        } else if (angle >= 110 && angle <= 160)
        {
            cardinal = 3;
        } else if (Mathf.Abs(angle) > 160)
        {
            cardinal = 4;
        } else if (angle >= -160 && angle <= -110)
        {
            cardinal = 5;
        } else if (angle > -110 && angle < -70)
        {
            cardinal = 6;
        } else if (angle >= -70 && angle <= -20)
        {
            cardinal = 7;
        }

        if (detectedInd == 6)
        {
            speaker.clip = detectedSpeak[detectedInd];
        } else
        {
            speaker.clip = CombineClips(detectedSpeak[detectedInd], cardinalSpeak[cardinal]);
        }

        speaker.Play();
    }

    public void UpdateSonar()
    {
        updates++;
        beepSpeed += 0.05f;
    }

    void Tick(Vector3 pos)
    {
        GameObject tickObj = new GameObject("Tick"); 
        tickObj.transform.position = pos; 

        AudioSource tickAS = tickObj.AddComponent<AudioSource>();
        tickAS.spatialBlend = 1;
        AudioHighPassFilter tickHigh = tickObj.AddComponent<AudioHighPassFilter>();
        AudioLowPassFilter tickLow = tickObj.AddComponent<AudioLowPassFilter>();

        tickHigh.cutoffFrequency = tickPitch;
        tickLow.cutoffFrequency = 1000f + (6000f - tickPitch)*3;
        tickAS.PlayOneShot(tick);

        //AudioSource cardinalAS = tickObj.AddComponent<AudioSource>();
        //cardinalAS.spatialBlend = 1;
        //cardinalAS.volume = 0.5f;

        //AudioClip cardinal = cardinalSpeak[1];

        //cardinalAS.PlayOneShot(cardinal);

        Destroy(tickObj, tick.length);
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

        int pitch = (GameManager.lang == 0) ? 50000 : 15000;

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, pitch, false);
        result.SetData(data, 0);

        return result;
    }
}
