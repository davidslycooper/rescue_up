using UnityEngine.SceneManagement;
using UnityEngine;

public class Menus : MonoBehaviour
{
    int scene;
    public AudioClip[] menuOptionsEng;
    public AudioClip[] menuOptionsPt;

    AudioClip[] menuOptions;

    public AudioClip titleEng;
    public AudioClip titlePt;

    AudioClip title;

    int menuOption;

    AudioSource speaker;

    public AudioClip menuSwipe;
    public AudioClip transit;
    public AudioClip block;

    void Start()
    {
        AudioSource.PlayClipAtPoint(transit, transform.position + new Vector3(0, 0, -10));

        scene = SceneManager.GetActiveScene().buildIndex;

        title = (GameManager.lang == 0) ? titleEng : titlePt;
        menuOptions = (GameManager.lang == 0) ? menuOptionsEng : menuOptionsPt;

        menuOption = -1;

        speaker = GetComponent<AudioSource>();

        if (GameManager.narrator)
        {
            if (scene == GameManager.mainMenuScene || scene == GameManager.languageMenuScene || scene == GameManager.narratorOptionsScene || (scene == GameManager.warnControlsScene && !GameManager.warned))
            {
                speaker.clip = title;
                speaker.Play();
            }
            if (scene == GameManager.winScene || scene == GameManager.deathScene)
            {
                speaker.clip = menuOptions[0];
                speaker.PlayDelayed(3);
                menuOption = 0;
            }
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
                AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(-0.01f, 0, -10));
            }
            else
            {
                AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(-0.01f, 0, -10));
            }
            Say();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            menuOption++;

            if (menuOption > menuOptions.Length - 1)
            {
                menuOption = menuOptions.Length - 1;
                AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0.01f, 0, -10));
            }
            else
            {
                AudioSource.PlayClipAtPoint(menuSwipe, transform.position + new Vector3(0.01f, 0, -10));
            }
            Say();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (menuOption >= 0)
            {
                Do();
            } else
            {
                AudioSource.PlayClipAtPoint(block, transform.position + new Vector3(0.01f, 0, -10));
                menuOption = Mathf.Min(menuOption + 1, 1);
                Say();
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
        if (scene == GameManager.languageMenuScene)
        {
            switch (menuOption)
            {
                case 0:
                    GetComponent<LanguageMenu>().English();
                    break;
                case 1:
                    GetComponent<LanguageMenu>().Portuguese();
                    break;
            }
        }
        else if (scene == GameManager.narratorOptionsScene)
        {
            switch (menuOption)
            {
                case 0:
                    GetComponent<NarratorMenuManager>().Enable();
                    break;
                case 1:
                    GetComponent<NarratorMenuManager>().Disable();
                    break;
            }
        }
        else if (scene == GameManager.mainMenuScene)
        {
            switch (menuOption)
            {
                case 0:
                    GetComponent<MainMenu>().TutorialStartEngin();
                    break;
                case 1:
                    GetComponent<MainMenu>().TutorialStartPilot();
                    break;
                case 2:
                    GetComponent<MainMenu>().PlayStartEngin();
                    break;
                case 3:
                    GetComponent<MainMenu>().PlayStartPilot();
                    break;
                case 4:
                    GetComponent<MainMenu>().Options();
                    break;
                case 5:
                    GetComponent<MainMenu>().QuitGame();
                    break;
            }
        }
        else if (scene == GameManager.menuOptionsScene)
        {
            switch (menuOption)
            {
                case 0:
                    GetComponent<MenuOptions>().Language();
                    break;
                case 1:
                    GetComponent<MenuOptions>().Controls();
                    break;
                case 2:
                    GetComponent<MenuOptions>().Narration();
                    break;
                case 3:
                    GetComponent<MenuOptions>().Back();
                    break;
            }
        }
        else if (scene == GameManager.warnControlsScene)
        {
            switch (menuOption)
            {
                case 0:
                    GetComponent<WarnControls>().Default();
                    break;
                case 1:
                    GetComponent<WarnControls>().Alternative();
                    break;
            }
        }
        else if (scene == GameManager.winScene || scene == GameManager.deathScene)
        {
            switch (menuOption)
            {
                case 0:
                    Say();
                    break;
                case 1:
                    GetComponent<Death>().Continue();
                    break;
            }
        }
    }
}
