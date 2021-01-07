using UnityEngine;
using TMPro;

public class ButtonTextLang : MonoBehaviour
{
    public string eng;
    public string pt;

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = (GameManager.lang == 0) ? eng : pt;
    }
}
