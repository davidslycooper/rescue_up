using UnityEngine;
using TMPro;

public class TextInstructionAlternate : MonoBehaviour
{
    public string eng;
    public string pt;
    public string eng_alt;
    public string pt_alt;

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = (GameManager.lang == 0) ? ((!GameManager.alternate)? eng : eng_alt): (!GameManager.alternate) ? pt : pt_alt;
    }
}
