using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideButton : MonoBehaviour
{
    public bool tutorial;
    public bool pilot;

    void Start()
    {
        if (tutorial && GameManager.tutorial)
        {
            Hide();
        }
        else if (pilot && GameManager.pilot)
        {
            Hide();
        }
    }

    void Hide()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }
}
