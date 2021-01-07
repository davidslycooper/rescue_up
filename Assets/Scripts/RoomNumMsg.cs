using UnityEngine;
using TMPro;

public class RoomNumMsg : MonoBehaviour
{
    string eng = "ROOM NUMBER IS ";
    string pt = "O NÚMERO DA SALA É ";

    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = (GameManager.lang == 0) ? eng + GameManager.roomNum: pt + GameManager.roomNum;
    }
}
