using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Diagnostics;

public class PlayerLog : MonoBehaviourPunCallbacks
{

    public static PlayerLog log;

    string sessionId;
    string seed;

    float pauseStart;

    float startedGame;
    float startedTutorial;
    float startedMission;
    float startedStep;

    int steps;
    float stepTimes;

    int coalSpawned;
    int uraniumSpawned;
    int quartzSpawned;
    int amberSpawned;
    int enemiesSpawned;
    int brokenRocksSpawned;

    int chases;
    float startedChase;
    float chaseTimes;


    // PILOT
    int coalCaught;
    int uraniumCaught;
    int quartzCaught;
    int amberCaught;
    bool treasureCaught;

    int caughts;

    int stateChanges;
    float saveTimes;
    float collectTimes;
    float flareTimes;
    float cannonTimes;

    int flares;
    int topFire;
    int botFire;
    int fires;
    int brokenRocks;
    int enemyHits;
       

    // ENGINEER
    int pulses;
    float pulseTimes;
    int infos;
    int coalDetect;
    int uraniumDetect;
    int quartzDetect;
    int amberDetect;
    bool treasureDetect;
    int enemiesDetect;
    int detections;
    int charges;
    int upgrades;
    int flaresBuilt;
    int torpedoesBuilt;
    int flaresLoaded;
    int topLoaded;
    int botLoaded;
    int loaded;
    int navigation;

    List<int> coalDetected;
    List<int> uraniumDetected;
    List<int> quartzDetected;
    List<int> amberDetected;
    List<int> enemiesDetected;

    bool inChase;
    bool inGame;

    void Start()
    {
        if (log == null)
        {
            log = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (log != this) 
        {
            Destroy(gameObject);
        }

        coalDetected = new List<int>();
        uraniumDetected = new List<int>();
        quartzDetected = new List<int>();
        amberDetected = new List<int>();
        enemiesDetected = new List<int>();
    }

    public override void OnConnectedToMaster()
    {
        Send(false, "General Log", "Connected", "Ping", PhotonNetwork.GetPing().ToString());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Send(false, "General Log", "Disconnected", "Ping", PhotonNetwork.GetPing().ToString());
    }

    public void StartGame() //
    {
        startedGame = Time.time;

        string reader = "None";

        try
        {
            Process[] ps = Process.GetProcesses();

            foreach (Process p in ps)
            {
                UnityEngine.Debug.Log(p.ProcessName);
                if (p.ProcessName == "nvda") { reader = "NVDA"; break; }
                else if (p.ProcessName == "jaws") { reader = "JAWS"; break; }
                else if (p.ProcessName == "Magnify") { reader = "Windows Magnify"; break; }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Erro");
        }

        Send(false, "General Log", "Boot", "Screen Reader", reader);
    }

    public void QuitGame() //
    {
        float time = Mathf.Round((Time.time - startedGame) * 10f) / 10f;

        string type = "General Log";
        string msg = "Quit";

        if (inGame)
        {
            if (GameManager.tutorial) 
            {
                TutorialReport();
                msg = "Forced Quit Tutorial";
                time = Mathf.Round((Time.time - startedTutorial) * 10f) / 10f;
            }
            else
            {
                MissionReport();
                msg = "Forced Quit Mission";
                time = Mathf.Round((Time.time - startedMission) * 10f) / 10f;
            }
            type = "Session Report";
        }

        Send(inGame, type, msg, "Time", time.ToString());
    }

    public void StartTutorial() //
    {
        startedTutorial = Time.time;
        steps = 0;
        stepTimes = 0;

        sessionId = Session();

        inGame = true;

        Send(true, "Session Report", "Started Tutorial", "Role", (GameManager.pilot)? "Pilot": "Engineer");
        Send(true, "Session Report", "Started Tutorial", "Control Scheme", (GameManager.alternate) ? "2" : "1");
        Send(true, "Session Report", "Started Tutorial", "Narrator", (GameManager.narrator) ? "Enabled" : "Disabled");
    }

    public void NewStep(int step) //
    {
        startedStep = Time.time;
    }

    public void EndStep(int step) //
    { 
        float time = Mathf.Round((Time.time - startedStep) * 10f) / 10f;

        stepTimes += time;
        steps++;

        Send(true, "Session Report", "Finished Tutorial Step", "Time", time.ToString(), (step + 1).ToString());
    }

    public void EndTutorial() //
    {
        float time = Mathf.Round((Time.time - startedTutorial) * 10f) / 10f;

        TutorialReport();

        inGame = false;

        Send(true, "Session Report", "Finished Tutorial", "Time", time.ToString());
    }

    public void StartMission(string seed) //
    {
        startedMission = Time.time;

        sessionId = Session();

        coalSpawned = 0;
        uraniumSpawned = 0;
        quartzSpawned = 0;
        amberSpawned = 0;
        enemiesSpawned = 0;
        brokenRocksSpawned = 0;

        inChase = false;
        chaseTimes = 0;

        if (GameManager.pilot)
        {
            coalCaught = 0;
            uraniumCaught = 0;
            quartzCaught = 0;
            amberCaught = 0;
            treasureCaught = false;

            caughts = 0;

            stateChanges = 0;
            saveTimes = 0;

            flares = 0;
            topFire = 0;
            botFire = 0;
            fires = 0;
            brokenRocks = 0;
            enemyHits = 0;
        }
        else
        {
            pulses = 0;
            pulseTimes = 0;
            infos = 0;
            coalDetect = 0;
            uraniumDetect = 0;
            quartzDetect = 0;
            amberDetect = 0;
            enemiesDetect = 0;
            treasureDetect = false;
            detections = 0;
            charges = 0;
            upgrades = 0;
            flaresBuilt = 0;
            torpedoesBuilt = 0;
            flaresLoaded = 0;
            topLoaded = 0;
            botLoaded = 0;
            loaded = 0;
            navigation = 0;
            coalDetected.Clear();
            uraniumDetected.Clear();
            quartzDetected.Clear();
            amberDetected.Clear();
            enemiesDetected.Clear();
        }

        inGame = true;

        Send(true, "Session Report", "Started Mission", "Role", (GameManager.pilot) ? "Pilot" : "Engineer");
        Send(true, "Session Report", "Started Mission", "Control Scheme", (GameManager.alternate) ? "2" : "1");
        Send(true, "Session Report", "Started Tutorial", "Narrator", (GameManager.narrator) ? "Enabled" : "Disabled");
        Send(true, "Session Report", "Started Mission", "Seed", seed);
    }

    public void Die(int id, Vector3 pos) //
    {
        if (inChase)
        {
            float chasetime = Time.time - startedChase;
            chaseTimes += chasetime;
        }

        MissionReport();

        float time = Mathf.Round((Time.time - startedMission) * 10f) / 10f;

        if (GameManager.deathCause)
        {
            Send(true, "Session Report", "Failed Mission", "Cause", "Battery");
        }
        else
        {
            Send(true, "Session Report", "Failed Mission", "Cause", "Devoured");
            Send(true, "Session Report", "Failed Mission", "Enemy", (id + 1).ToString());
        }

        Send(true, "Session Report", "Failed Mission", "Position", "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y* 10f) / 10f + ")");
        Send(true, "Session Report", "Failed Mission", "Time", time.ToString());

        inGame = false;
    }

    public void Win() //
    {
        if (inChase)
        {
            float chasetime = Time.time - startedChase;
            chaseTimes += chasetime;
        }

        MissionReport();

        float time = Mathf.Round((Time.time - startedMission) * 10f) / 10f;

        Send(true, "Session Report", "Succeed Mission", "Time", time.ToString());

        inGame = false;
    }

    public void QuitFromPause() //
    {
        if (GameManager.tutorial)
        {
            TutorialReport();

            float time = Mathf.Round((Time.time - startedTutorial) * 10f) / 10f;

            Send(true, "Session Report", "Left Tutorial", "Time", time.ToString());
        }
        else
        {
            MissionReport();

            float time = Mathf.Round((Time.time - startedMission) * 10f) / 10f;

            Send(true, "Session Report", "Left Mission", "Time", time.ToString());
        }

        inGame = false;
    }

    public void Pause() //
    {
        pauseStart = Time.time;
    }

    public void Resume() //
    {
        float time = Mathf.Round((Time.time - pauseStart) * 10f) / 10f;

        Send(true, "Session Report", "Pause", "Time", time.ToString());
    }

    public void Instructions() //
    {
        Send(true, "Session Report", "Consulted Instructions");
    }

    public void ChangedControls(bool alternate) //
    {
        Send(true, "Session Report", "Changed Controls", "Control Scheme", (alternate)? "2": "1");
    }

    string Session()
    {
        DateTime date = DateTime.Now;
        return date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
    }

    void TutorialReport()
    {
        Send(true, "Session Report", "Tutorial Summary", "Avg Time Per Step", (Mathf.Round(stepTimes /steps * 10f) / 10f).ToString());
    }

    void MissionReport()
    {
        if (GameManager.pilot)
        {
            Send(true, "Session Report", "Mission Summary", "Coal Spawned", coalSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Uranium Spawned", uraniumSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Quartz Spawned", quartzSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Amber Spawned", amberSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Enemies Spawned", enemiesSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Broken Rocks Spawned", brokenRocksSpawned.ToString());
            Send(true, "Session Report", "Mission Summary", "Chases", chases.ToString());
            Send(true, "Session Report", "Mission Summary", "Chase Time", (Mathf.Round(chaseTimes * 10f) / 10f).ToString());

            PilotReport();
        }
        else
        {
            SonarReport();
        }
    }

    void PilotReport()
    {
        Send(true, "Session Report", "Pilot Summary", "Caughts", caughts.ToString());
        Send(true, "Session Report", "Pilot Summary", "Coal Caught", coalCaught.ToString());
        Send(true, "Session Report", "Pilot Summary", "Uranium Caught", uraniumCaught.ToString());
        Send(true, "Session Report", "Pilot Summary", "Quartz Caught", quartzCaught.ToString());
        Send(true, "Session Report", "Pilot Summary", "Amber Caught", amberCaught.ToString());
        Send(true, "Session Report", "Pilot Summary", "Treasure Caught", (treasureCaught)? "Caught": "Not Caught");
        Send(true, "Session Report", "Pilot Summary", "State Changes", stateChanges.ToString());
        Send(true, "Session Report", "Pilot Summary", "Save Mode Time", (Mathf.Round(saveTimes * 10f) / 10f).ToString());
        Send(true, "Session Report", "Pilot Summary", "Collect Mode Time", (Mathf.Round(collectTimes * 10f) / 10f).ToString());
        Send(true, "Session Report", "Pilot Summary", "Flare Mode Time", (Mathf.Round(flareTimes * 10f) / 10f).ToString());
        Send(true, "Session Report", "Pilot Summary", "Torpedo Mode Time", (Mathf.Round(cannonTimes * 10f) / 10f).ToString());

        Send(true, "Session Report", "Pilot Summary", "Flares Shot", flares.ToString());
        Send(true, "Session Report", "Pilot Summary", "Torpedoes Shot", fires.ToString());
        Send(true, "Session Report", "Pilot Summary", "Torpedoes Shot (Top)", topFire.ToString());
        Send(true, "Session Report", "Pilot Summary", "Torpedoes Shot (Bottom)", botFire.ToString());
        Send(true, "Session Report", "Pilot Summary", "Broken Rock Hits", brokenRocks.ToString());
        Send(true, "Session Report", "Pilot Summary", "Enemy Hits", enemyHits.ToString());
    }

    void SonarReport()
    {
        Send(true, "Session Report", "Engineer Summary", "Pulses", pulses.ToString());

        Send(true, "Session Report", "Engineer Summary", "Coal Detected", coalDetect.ToString());
        Send(true, "Session Report", "Engineer Summary", "Uranium Detected", uraniumDetect.ToString());
        Send(true, "Session Report", "Engineer Summary", "Quartz Detected", quartzDetect.ToString());
        Send(true, "Session Report", "Engineer Summary", "Amber Detected", amberDetect.ToString());
        Send(true, "Session Report", "Engineer Summary", "Enemies Detected", enemiesDetect.ToString());
        Send(true, "Session Report", "Engineer Summary", "Treasure Detected", (treasureDetect)? "Detected": "Not Detected");
        Send(true, "Session Report", "Engineer Summary", "Total Detections", detections.ToString());

        Send(true, "Session Report", "Engineer Summary", "Battery Charges", charges.ToString());
        Send(true, "Session Report", "Engineer Summary", "Sonar Upgrades", upgrades.ToString());
        Send(true, "Session Report", "Engineer Summary", "Flares Built", flaresBuilt.ToString());
        Send(true, "Session Report", "Engineer Summary", "Flares Loaded", flaresLoaded.ToString());
        Send(true, "Session Report", "Engineer Summary", "Torpedoes Built", torpedoesBuilt.ToString());
        Send(true, "Session Report", "Engineer Summary", "Torpedoes Loaded (Top)", topFire.ToString());
        Send(true, "Session Report", "Engineer Summary", "Torpedoes Loaded (Bottom)", botFire.ToString());
        Send(true, "Session Report", "Engineer Summary", "Navigations", navigation.ToString());

        Send(true, "Session Report", "Engineer Summary", "Avg Time Pulse", (Mathf.Round(pulseTimes / pulses * 10f) / 10f).ToString());
    }

    public void SpawnSurface(Vector3 pos) 
    {
        Send(true, "Session Report", "Surface Spawned", "Position", "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")");
    }

    public void SpawnTreasure(Vector3 pos)
    {
        Send(true, "Session Report", "Treasure Spawned", "Position", "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")");
    }

    public void SpawnCoal() //
    {
        coalSpawned++;
    }

    public void SpawnUranium() //
    {
        uraniumSpawned++;
    }

    public void SpawnQuartz() //
    {
        quartzSpawned++;
    }

    public void SpawnAmber() //
    {
        amberSpawned++;
    }

    public void SpawnEnemy(int id, Vector3 pos) //
    {
        enemiesSpawned++;
        Send(true, "Session Report", "Enemy Spawned", "Position", "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", (id + 1).ToString());
    }

    public void SpawnBrokenRock(Vector3 pos) //
    {
        brokenRocksSpawned++;
        Send(true, "Session Report", "BrokenRock Spawned", "Position", "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", brokenRocksSpawned.ToString());
    }

    public void StartChase(int id, Vector3 enemyPos, Vector3 subPos) //
    {
        if (!inChase)
        {
            startedChase = Time.time;
            inChase = true;
        }

        chases++;
        Send(true, "Session Report", "Start Chase", "Position", 
            "(" + Mathf.Round(enemyPos.x * 10f) / 10f + ";" + Mathf.Round(enemyPos.y * 10f) / 10f + ")"
            + " ---> (" + Mathf.Round(subPos.x * 10f) / 10f + ";" + Mathf.Round(subPos.y * 10f) / 10f + ")", 
            (id + 1).ToString());
    }

    public void StopChase(int id) //
    {
        float time = Time.time - startedChase;
        chaseTimes += time;
        inChase = false;

        Send(true, "Session Report", "End Chase", "Time", time.ToString(), (id + 1).ToString());
    }

    public void Catch(int id, int type) // 
    {
        string caught = "";

        switch (type)
        {
            case 0:
                coalCaught++;
                caught = "Coal";
                break;
            case 1:
                uraniumCaught++;
                caught = "Uranium";
                break;
            case 2:
                quartzCaught++;
                caught = "Quartz";
                break;
            case 3:
                amberCaught++;
                caught = "Amber";
                break;
            case 4:
                treasureCaught = true;
                caught = "Treasure";
                break;
        }

        caughts++;

        Send(true, "Session Report", caught + " Caught", "", "", (id + 1).ToString());
    }

    public void ChangeState(int prevState, int state, float time) //
    {
        string mode = "";

        switch (prevState)
        {
            case 0:
                collectTimes += time;
                break;
            case 1:
                flareTimes += time;
                break;
            case 2:
                cannonTimes += time;
                break;
            case 3:
                saveTimes += time;
                break;

        }

        switch (state)
        {
            case 0:
                mode = "Collect";
                break;
            case 1:
                mode = "Flare";
                break;
            case 2:
                mode = "Shoot";
                break;
            case 3:
                mode = "Save";
                break;

        }

        stateChanges++;

        Send(true, "Session Report", "Changed State", "Mode", mode, stateChanges.ToString());
    }

    public void Sign(Vector3 pos) //
    {
        flares++;

        Send(true, "Session Report", "Person Delivered", "Position", 
            "(" + Mathf.Round(pos.x* 10f) / 10f + ";" + Mathf.Round(pos.y* 10f) / 10f + ")", flares.ToString());
    }

    public void Shoot(bool top, Vector3 pos) //
    {
        string cannon = "";

        if (top)
        {
            cannon = "Top";
            topFire++;
        }
        else
        {
            cannon = "Bottom";
            botFire++;
        }
        fires++;

        Send(true, "Session Report", "Torpedo Fired (" + cannon + "Cannon)", "Position",
            "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", fires.ToString());
    }

    public void BrokenRock(Vector3 pos) //
    {
        brokenRocks++;

        Send(true, "Session Report", "Rock Broken", "Position",
            "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", brokenRocks.ToString());
    }

    public void EnemyHit(int id, Vector3 pos) //
    {
        brokenRocks++;

        Send(true, "Session Report", "Enemy Hit", "Position",
            "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", id.ToString());
    }

    /////////////////////////////////
    

    public void Pulse(float time, Vector3 pos, float direction) //
    {
        pulses++;
        pulseTimes += time;

        Send(true, "Session Report", "Pulse", "Position",
            "(" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ") / " + Mathf.Round(direction), pulses.ToString());
    }

    public void Info() //
    {
        infos++;
    }

    public void Detected(int type, int id, Vector3 subPos, Vector3 pos) //
    {
        detections++;

        string detect = "";

        switch (type)
        {
            case 0:
                if (!(coalDetected.Contains(id)))
                {
                    coalDetected.Add(id);
                    coalDetect++;
                }
                detect = "Coal";
                break;
            case 1:
                if (!(uraniumDetected.Contains(id)))
                {
                    uraniumDetected.Add(id);
                    uraniumDetect++;
                }
                detect = "Uranium";
                break;
            case 2:
                if (!(quartzDetected.Contains(id)))
                {
                    quartzDetected.Add(id);
                    quartzDetect++;
                }
                detect = "Quartz";
                break;
            case 3:
                if (!(amberDetected.Contains(id)))
                {
                    amberDetected.Add(id);
                    amberDetect++;
                }
                detect = "Amber";
                break;
            case 4:
                if (!treasureDetect)
                {
                    treasureDetect = true;
                }
                detect = "Treasure";
                break;
            case 5: // CONFIRMAR
                if (!(enemiesDetected.Contains(id)))
                {
                    enemiesDetected.Add(id);
                    enemiesDetect++;
                }
                detect = "Enemy";
                break;
        }

        detections++;

        Send(true, "Session Report", detect + " Detected", "Position",
            "(" + Mathf.Round(subPos.x * 10f) / 10f + ";" + Mathf.Round(subPos.y * 10f) / 10f + ")"
            + " ---> (" + Mathf.Round(pos.x * 10f) / 10f + ";" + Mathf.Round(pos.y * 10f) / 10f + ")", id.ToString());
    }

    public void ChargeBattery(int prevBatt, int battery) //
    {
        charges++;

        Send(true, "Session Report", "Battery Charged", "Level",
            prevBatt * 10 + " ---> " + battery * 10, charges.ToString());
    }

    public void UpgradeSonar(int range) //
    {
        upgrades++;

        Send(true, "Session Report", "Sonar Upgraded", "Level",
            (range - 1) * 10 + " ---> " + range * 10, upgrades.ToString());
    }

    public void BuildFlare() //
    {
        flaresBuilt++;

        Send(true, "Session Report", "Flare Built", "", "", flaresBuilt.ToString());
    }

    public void LoadFlare() //
    {
        flaresLoaded++;

        Send(true, "Session Report", "Flare Loaded", "", "", flaresLoaded.ToString());
    }

    public void BuildTorpedo() //
    {
        torpedoesBuilt++;

        Send(true, "Session Report", "Torpedo Built", "", "", torpedoesBuilt.ToString());
    }

    public void LoadTorpedo(bool top) //
    {
        string cannon = "";

        if (top)
        {
            topLoaded++;
        }
        else
        {
            botLoaded++;
        }

        loaded++;

        Send(true, "Session Report", "Torpedo Loaded", "Cannon", cannon, loaded.ToString());
    }

    public void Navigation() //
    {
        navigation++;
    }

    public void ConnectionError()
    {
        Send(true, "General Log", "Connection Error");
    }

    IEnumerator Post(bool session, string elemId, string type, string msg, string key, string val)
    {
        UnityEngine.Debug.Log("Sent");

        WWWForm form = new WWWForm();

        form.AddField("entry.254097327", GameManager.id);
        form.AddField("entry.2006021226", (session)? sessionId: ""); 
        form.AddField("entry.2034932941", elemId);
        form.AddField("entry.464647826", type);
        form.AddField("entry.365802895", msg);
        form.AddField("entry.1474826894", key);
        form.AddField("entry.1779893975", val);

        byte[] rawData = form.data;

        WWW www = new WWW("https://docs.google.com/forms/u/0/d/e/1FAIpQLSe3giqGDRKgVX0-NhtnFQAESoVcBQ4E2rWetZAuXWN2P78DkQ/formResponse", rawData);

        yield return www;
    }

    public void Send(bool session, string type, string msg, string key = "", string val = "", string elemId = "")
    {
        StartCoroutine(Post(session, elemId, type, msg, key, val));
    }
}
