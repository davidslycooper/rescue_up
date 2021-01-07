using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanel : MonoBehaviour
{

    bool tutorial;

    public int menuState;
    int optionSelected;

    int[] menuOptions;

    bool saveBattery;

    public int batteryLevel;
    int sonarRange;
    int flaresQt;
    int torpedoesQt;

    public int[] minerals; // 0 = Carvão; 1 = Urânio; 2 = Quartzo; 3 = Âmbar

    public AudioSource speaker;
    AudioClip speak;

    public AudioClip menuSwipe;
    public AudioClip sonarSwipe;
    public AudioClip block;

    public AudioClip charge;
    public AudioClip craft;
    public AudioClip loading;
    public AudioClip update;
    public AudioClip storage;

    ///

    public AudioClip[] numTenEng;
    public AudioClip[] numHundEng;

    public AudioClip[] optionsEng;

    public AudioClip[] mineralNamesEng;

    public AudioClip percentEng;
    public AudioClip[] batterySaveEng;

    public AudioClip rangeEng;
    public AudioClip metersEng;

    public AudioClip[] buildEng;

    public AudioClip[] builtEng;

    public AudioClip[] loadEng;
    public AudioClip[] loadedEng;

    public AudioClip warnMaxBatteryEng;
    public AudioClip[] warnMineralShortEng;
    public AudioClip[] warnItemShortEng;
    public AudioClip[] itemNamesEng;
    public AudioClip[] warnAlreadyLoadedEng;

    public AudioClip batteryUpdateEng;
    public AudioClip sonarUpdateEng;

    public AudioClip[] collectedEng;

    ///

    public AudioClip[] numTenPt;
    public AudioClip[] numHundPt;

    public AudioClip[] optionsPt;

    public AudioClip[] mineralNamesPt;

    public AudioClip percentPt;
    public AudioClip[] batterySavePt;

    public AudioClip rangePt;
    public AudioClip metersPt;

    public AudioClip[] buildPt;

    public AudioClip[] builtPt;

    public AudioClip[] loadPt;
    public AudioClip[] loadedPt;

    public AudioClip warnMaxBatteryPt;
    public AudioClip[] warnMineralShortPt;
    public AudioClip[] warnItemShortPt;
    public AudioClip[] itemNamesPt;
    public AudioClip[] warnAlreadyLoadedPt;

    public AudioClip batteryUpdatePt;
    public AudioClip sonarUpdatePt;

    public AudioClip[] collectedPt;

    ///

    AudioClip[] numTen;
    AudioClip[] numHund;

    AudioClip[] options; // 0 = Bateria; 1 = Sonar; 2 = Sinais de luz; 3 - Torpedos

    AudioClip[] mineralNames; // 0 = Carvão; 1 = Urânio; 2 = Quartzo; 3 = Âmbar

    AudioClip percent; // Por cento
    AudioClip[] batterySave; // 0 = O submarino está em modo poupança; 1 = O submarino não está em modo poupança;

    AudioClip range; // Alcance de
    AudioClip meters; // Metros

    AudioClip[] build; // 0 = Construir sinal de luz: Custa um de Âmbar; 1 = Construir torpedo: Custa um de Carvão

    AudioClip[] built; // 0 = Sinal de luz construído; 1 = Torpedo construído 

    AudioClip[] load; // 0 = Carregar Bateria com um de carvão; 1 = Carregar bateria com um de urânio; 2 = Melhorar o alcance do sonar com um de quartzo; 3 = Carregar sinalizador com sinal de luz; 4 = Carregar canhão de cima com torpedo; 5 = Carregar canhão de baixo com torpedo

    AudioClip[] loaded; // 0 = Sinalizador carregado; 1 = O Sinalizador não está carregado; 2 = Ambos os canhões estão carregados; 3 = Canhão de cima carregado; 4 = Canhão de baixo carregado; 5 = Nenhum dos canhões está carregado

    AudioClip warnMaxBattery; // A bateria já está no máximo
    AudioClip[] warnMineralShort; // 0 = Não tens; 1 = Suficiente
    AudioClip[] warnItemShort; // 0 = Não tens nenhum; 1 = Para carregar
    AudioClip[] itemNames; // 0 = Sinal de luz; 1 = Torpedo
    AudioClip[] warnAlreadyLoaded; // 0 = O sinalizador já está carregado; 1 = O canhão de cima já está carregado; 2 = O canhão de baixo já está carregado

    AudioClip batteryUpdate; // A bateria encontra-se agora a
    AudioClip sonarUpdate; // O sonar tem agora uma alcance de

    AudioClip[] collected; // 0 = Carvão coletado; 1 = Urânio coletado; 2 = Quartzo coletado; 3 = Âmbar coletado; 4 = Conseguiram coletar o tesouro

    [Range(0f, 100f)]
    public float batteryLasts;
    float batteryDecTime;
    int batteryTick;
    int batteryMax;

    SubManager subMan;
    ActiveSonar activeSonar;
    PassiveSonar passiveSonar;

    PlayerLog log;

    void Start()
    {
        activeSonar = GetComponentInChildren<ActiveSonar>();
        passiveSonar = GetComponentInChildren<PassiveSonar>();

        subMan = transform.root.gameObject.GetComponent<SubManager>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        minerals = new int[4];

        menuState = 0;

        menuOptions = new int[] { 0, 2, 1, 2, 3 };

        speaker = GetComponent<AudioSource>();

        batteryMax = 10;
        batteryTick = 1;
        batteryDecTime = 20f;
        saveBattery = true;
        if (GameManager.tutorial)
        {
            batteryLevel = 9;
        }
        else
        {
            batteryLevel = 10;
        }
        
        sonarRange = 4;

        batteryDecTime = Time.time + batteryLasts;

        AssignAudios();
    }

    public void AssignAudios()
    {
        if (GameManager.lang == 0)
        {
            numTen = numTenEng;
            numHund = numHundEng;
            options = optionsEng; 
            mineralNames = mineralNamesEng;
            percent = percentEng; 
            batterySave = batterySaveEng; 
            range = rangeEng; 
            meters = metersEng;
            build = buildEng;
            built = builtEng; 
            load = loadEng; 
            loaded = loadedEng;
            warnMaxBattery = warnMaxBatteryEng ;
            warnMineralShort = warnMineralShortEng; 
            warnItemShort = warnItemShortEng; 
            itemNames = itemNamesEng;
            warnAlreadyLoaded = warnAlreadyLoadedEng; 
            batteryUpdate = batteryUpdateEng; 
            sonarUpdate = sonarUpdateEng; 
            collected = collectedEng;
        } else
        {
            numTen = numTenPt;
            numHund = numHundPt;
            options = optionsPt;
            mineralNames = mineralNamesPt;
            percent = percentPt;
            batterySave = batterySavePt;
            range = rangePt;
            meters = metersPt;
            build = buildPt;
            built = builtPt;
            load = loadPt;
            loaded = loadedPt;
            warnMaxBattery = warnMaxBatteryPt;
            warnMineralShort = warnMineralShortPt;
            warnItemShort = warnItemShortPt;
            itemNames = itemNamesPt;
            warnAlreadyLoaded = warnAlreadyLoadedPt;
            batteryUpdate = batteryUpdatePt;
            sonarUpdate = sonarUpdatePt;
            collected = collectedPt;
        }
    }

    void Update()
    {
        if (!GameManager.tutorial)
        {
            if (!PanelNav())
            {
                if (!Action())
                {
                    Storage();
                }
            }
            Manage();
        }
    }

    public void Manage()
    {
        if (Time.time > batteryDecTime)
        {
            DecreaseBattery();
            batteryDecTime = Time.time + batteryLasts;
        }
    }

    public bool PanelNav(int panels = 4)
    {
        if (!PauseMenu.paused)
        {
            if (Input.GetKeyDown(KeyCode.A) || (!GameManager.alternate && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                menuState--;

                if (menuState < 1)
                {
                    menuState = 1;
                    AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(-5f, 0, 0));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(-0.01f, 0, 0));
                }
                optionSelected = 0;
                Say();

                if (!GameManager.tutorial)
                {
                    log.Navigation();
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.D) || (!GameManager.alternate && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                menuState++;

                if (menuState > panels)
                {
                    menuState = panels;
                    AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(5f, 0, 0));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0.01f, 0, 0));
                }
                optionSelected = 0;
                Say();

                if (!GameManager.tutorial)
                {
                    log.Navigation();
                }
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.S) || (!GameManager.alternate && Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (menuState == 0)
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0, -0.01f, 0));
                    menuState = 1;
                    Say();

                    if (!GameManager.tutorial)
                    {
                        log.Navigation();
                    }
                    return true;
                }
                else
                {
                    optionSelected--;

                    if (optionSelected < 0)
                    {
                        optionSelected = 0;
                        AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0, -5f, 0));
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0, -0.01f, 0));
                    }
                    Say();

                    if (!GameManager.tutorial)
                    {
                        log.Navigation();
                    }
                    return true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W) || (!GameManager.alternate && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if (menuState == 0)
                {
                    AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0, 0.01f, 0));
                    menuState = 1;
                    Say();

                    if (!GameManager.tutorial)
                    {
                        log.Navigation();
                    }
                    return true;
                }
                else
                {
                    optionSelected++;

                    if (optionSelected > menuOptions[menuState])
                    {
                        optionSelected = menuOptions[menuState];
                        AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0, 5f, 0));
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0, 0.01f, 0));
                    }
                    Say();

                    if (!GameManager.tutorial)
                    {
                        log.Navigation();
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool Action()
    {
        if (!PauseMenu.paused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (optionSelected == 0)
                {
                    Say();
                }
                else
                {
                    Do();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool Storage()
    {
        if (!PauseMenu.paused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // COAL
            {
                AudioSource.PlayClipAtPoint(storage, transform.position);

                speak = CombineClips(mineralNames[0], numTen[minerals[0]]);
                speaker.clip = speak;
                speaker.Play();
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // QUARTZ
            {
                AudioSource.PlayClipAtPoint(storage, transform.position);

                speak = CombineClips(mineralNames[2], numTen[minerals[2]]);
                speaker.clip = speak;
                speaker.Play();
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) // AMBER
            {
                AudioSource.PlayClipAtPoint(storage, transform.position);

                speak = CombineClips(mineralNames[3], numTen[minerals[3]]);
                speaker.clip = speak;
                speaker.Play();
                return true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && GameManager.uranium) // URANIUM
            {
                AudioSource.PlayClipAtPoint(storage, transform.position);

                speak = CombineClips(mineralNames[1], numTen[minerals[1]]);
                speaker.clip = speak;
                speaker.Play();
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void Pause()
    {
        speaker.Pause();
    }

    public void Unpause()
    {
        speaker.UnPause();
    }

    void Say()
    {
        switch (menuState)
        {
            case 0: // Minerais
                switch (optionSelected)
                {
                    case 0:
                        speak = CombineClips(options[0], mineralNames[0], numTen[minerals[0]], mineralNames[1], numTen[minerals[1]], mineralNames[2], numTen[minerals[2]], mineralNames[3], numTen[minerals[3]]); // mineral stock
                        break;
                }
                break;
            case 1: // Bateria
                switch (optionSelected)
                {
                    case 0:
                        speak = CombineClips(options[1], numHund[batteryLevel], percent, (saveBattery) ? batterySave[0] : batterySave[1]); // battery state
                        break;
                    case 1:
                        speak = load[0]; // charge battery with coal
                        break;
                    case 2:
                        speak = load[1]; // charge battery with uranium
                        break;
                }
                break;
            case 2: // Sonar
                switch (optionSelected)
                {
                    case 0:
                        speak = CombineClips(options[2], range, numHund[sonarRange], meters); // sonar distance
                        break;
                    case 1:
                        speak = load[2]; // update sonar with quartz
                        break;
                }
                break;
            case 3: // Sinais de luz
                switch (optionSelected)
                {
                    case 0:
                        speak = CombineClips(options[3], numTen[flaresQt], (subMan.flareLoaded) ? loaded[0] : loaded[1]); // flares qty
                        break;
                    case 1:
                        speak = build[0]; // build flare with amber
                        break;
                    case 2:
                        speak = load[3]; // load flare
                        break;
                }
                break;
            case 4: // Torpedos
                switch (optionSelected)
                {
                    case 0:
                        speak = CombineClips(options[4], numTen[torpedoesQt], ((subMan.topLoaded && subMan.botLoaded) ? loaded[2] : (subMan.topLoaded) ? loaded[3] : (subMan.botLoaded) ? loaded[4] : loaded[5])); // torpedoes qty
                        break;
                    case 1:
                        speak = build[1]; // build torpedo with two coal
                        break;
                    case 2:
                        speak = load[5]; // load top cannon
                        break;
                    case 3:
                        speak = load[4]; // load top cannon
                        break;
                }
                break;
        }

        speaker.clip = speak;
        speaker.Play();
    }

    void Do()
    {
        switch (menuState)
        {
            case 1: // Bateria
                switch (optionSelected)
                {
                    case 1:
                        ChargeBattery(1);
                        break;
                    case 2:
                        ChargeBattery(3);
                        break;
                }
                break;
            case 2: // Sonar
                switch (optionSelected)
                {
                    case 1:
                        UpdateSonar();
                        break;
                }
                break;
            case 3: // Sinais de luz
                switch (optionSelected)
                {
                    case 1:
                        BuildFlare();
                        break;
                    case 2:
                        LoadSigner();
                        break;
                }
                break;
            case 4: // Torpedos
                switch (optionSelected)
                {
                    case 1:
                        BuildTorpedo();
                        break;
                    case 2:
                        LoadCannon(false);
                        break;
                    case 3:
                        LoadCannon(true);
                        break;
                }
                break;
        }
    }

    public void ChangeState(bool changeToSave)
    {
        if (!changeToSave && saveBattery)
        {
            batteryLasts = batteryLasts / 2;
            batteryDecTime -= batteryLasts;
            saveBattery = false;

        }
        else if (changeToSave && !saveBattery)
        {
            batteryLasts = batteryLasts * 2;
            batteryDecTime += batteryLasts / 2;
            saveBattery = true;
        }
    }

    public void CatchMineral(int mineralType)
    {
        activeSonar.Caught(mineralType);

        if (mineralType == 4)
        {
            // CatchTreasure();
        }
        else
        {
            minerals[mineralType] = Mathf.Min(10, minerals[mineralType] + 1); // ALDRABADO
        }
        if (mineralType == 1 && !GameManager.uranium)
        {
            GameObject.FindWithTag("Helper").GetComponent<Helper>().ExplainUranium();
        } 
        else
        {
            speak = collected[mineralType];
            speaker.clip = speak;
            speaker.Play();
        }
    }

    //void CatchTreasure()
    //{
    //    subMan.CatchTreasure();
    //}

    void DecreaseBattery()
    {
        batteryLevel -= batteryTick;
        subMan.DecBattery();

        if (batteryLevel == 0)
        {
            subMan.Die(true);
        }
    }

    void ChargeBattery(int power)
    {
        if (power == 1)
        {
            if (minerals[0] > 0)
            {
                if (batteryLevel != batteryMax)
                {
                    if (!GameManager.tutorial)
                    {
                        log.ChargeBattery(batteryLevel, batteryLevel + power);
                    }

                    subMan.ChargeBattery(power);
                    batteryLevel += batteryTick * power;
                    minerals[0]--;

                    AudioSource.PlayClipAtPoint(charge, transform.position);

                    speaker.clip = CombineClips(batteryUpdate, numHund[batteryLevel], percent);
                    speaker.Play();
                }
                else
                {
                    WarnMaxBattery();
                }
            }
            else
            {
                WarnMineralShort(0);
            }
        }
        else if (power > 1)
        {
            if (minerals[1] > 0)
            {
                if (batteryLevel != batteryMax)
                {
                    if (!GameManager.tutorial)
                    {
                        log.ChargeBattery(batteryLevel, batteryLevel + power);
                    }

                    subMan.ChargeBattery(power);
                    batteryLevel += batteryTick * power;
                    minerals[1]--;

                    AudioSource.PlayClipAtPoint(charge, transform.position);

                    speaker.clip = CombineClips(batteryUpdate, numHund[batteryLevel], percent);
                    speaker.Play();
                }
                else
                {
                    WarnMaxBattery();
                }
            }
            else
            {
                WarnMineralShort(1);
            }
        }
    }

    void UpdateSonar()
    {
        if (minerals[2] > 0)
        {
            if (!GameManager.tutorial)
            {
                log.ChargeBattery(sonarRange, sonarRange + 1);
            }

            activeSonar.UpdateSonar();
            passiveSonar.UpdateSonar();
            sonarRange = Mathf.Min(10, sonarRange + 1); // ALDRABADO
            minerals[2]--;

            AudioSource.PlayClipAtPoint(update, transform.position);

            speaker.clip = CombineClips(sonarUpdate, numHund[sonarRange], meters);
            speaker.Play();
        }
        else
        {
            WarnMineralShort(2);
        }
    }

    void BuildFlare()
    {
        if (minerals[3] > 0)
        {
            if (!GameManager.tutorial)
            {
                log.BuildFlare();
            }

            flaresQt = Mathf.Min(10, flaresQt + 1); // ALDRABADO
            minerals[3]--;

            AudioSource.PlayClipAtPoint(craft, transform.position);

            speaker.clip = built[0];
            speaker.Play();
        }
        else
        {
            WarnMineralShort(3);
        }
    }

    void BuildTorpedo()
    {
        if (minerals[0] > 0)
        {
            if (!GameManager.tutorial)
            {
                log.BuildTorpedo();
            }

            torpedoesQt = Mathf.Min(10, torpedoesQt + 1); // ALDRABADO
            minerals[0] -= 1;

            AudioSource.PlayClipAtPoint(craft, transform.position);

            speaker.clip = built[1];
            speaker.Play();
        }
        else
        {
            WarnMineralShort(0);
        }
    }

    void LoadSigner()
    {
        if (!subMan.flareLoaded)
        {
            if (flaresQt > 0)
            {
                if (!GameManager.tutorial)
                {
                    log.LoadFlare();
                }

                subMan.LoadSigner();

                flaresQt--;

                AudioSource.PlayClipAtPoint(loading, transform.position);

                speaker.clip = loaded[0];
                speaker.Play();
            }
            else
            {
                WarnItemShort(0);
            }

        }
        else
        {
            WarnAlreadyLoaded(false);
        }
    }

    void LoadCannon(bool top)
    {
        if (top)
        {
            if (!subMan.topLoaded)
            {
                if (torpedoesQt > 0)
                {
                    if (!GameManager.tutorial)
                    {
                        log.LoadTorpedo(top);
                    }

                    subMan.LoadCannon(top);

                    torpedoesQt--;

                    AudioSource.PlayClipAtPoint(loading, transform.position);

                    speaker.clip = loaded[3];
                    speaker.Play();
                }
                else
                {
                    WarnItemShort(1);
                }
            }
            else
            {
                WarnAlreadyLoaded(true, true);
            }
        }
        else
        {
            if (!subMan.botLoaded)
            {
                if (torpedoesQt > 0)
                {
                    subMan.LoadCannon(top);

                    torpedoesQt--;

                    AudioSource.PlayClipAtPoint(loading, transform.position);

                    speaker.clip = loaded[4];
                    speaker.Play();
                }
                else
                {
                    WarnItemShort(1);
                }
            }
            else
            {
                WarnAlreadyLoaded(true, false);
            }
        }
    }

    void WarnMineralShort(int mineralType)
    {
        speaker.clip = CombineClips(warnMineralShort[0], mineralNames[mineralType], warnMineralShort[1]);
        speaker.Play();
    }

    void WarnItemShort(int itemType)
    {
        speaker.clip = CombineClips(warnItemShort[0], itemNames[itemType], warnItemShort[1]);
        speaker.Play();
    }

    void WarnMaxBattery()
    {
        speaker.clip = warnMaxBattery;
        speaker.Play();
    }

    void WarnAlreadyLoaded(bool cannon, bool top = false)
    {
        if (cannon)
        {
            if (top)
            {
                speaker.clip = warnAlreadyLoaded[1];
                speaker.Play();
            }
            else
            {
                speaker.clip = warnAlreadyLoaded[2];
                speaker.Play();
            }
        }
        else
        {
            speaker.clip = warnAlreadyLoaded[0];
            speaker.Play();
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

        int pitch = (GameManager.lang == 0) ? 50000 : 15000;

        AudioClip result = AudioClip.Create("Combine", length / 2, 2, pitch, false);
        result.SetData(data, 0);

        return result;
    }
}
