using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    List<string> ids;
    TMP_InputField id;

    void Awake()
    {
        id = GameObject.Find("ID").GetComponent<TMP_InputField>();
        string[] idsArr = new string[] { "3999", "0917", "9498", "3593", "8244", "9049", "5767", "9889", "9949", "7382", "1848", "3136", "1267", "0487", "4895", "2597", "9199", "4308", "1273", "6553", "5237", "7621", "1847",
            "3896", "1534", "0671", "5475", "3022", "4698", "7122", "1558", "0946", "9454", "9298", "1202", "7262", "3517", "0909", "4177", "3524", "8315", "0218", "4302", "6935", "7952", "8818", "7318", "8245", "2289", "0834", "3664",
            "3947", "8825", "0815", "4350", "7268", "7855", "1934", "0379", "5979", "1111", "2222" };
        ids = idsArr.ToList();
    }

    public void Login()
    {
        if (ids.Contains(id.text))
        {
            GameManager.id = id.text;
            SceneManager.LoadScene(GameManager.loadingScene);
        }
        else
        {
            GetComponent<MenusLogin>().WarnIncorrectID();
        }
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
