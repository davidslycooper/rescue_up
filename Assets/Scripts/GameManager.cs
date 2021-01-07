using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.IO;

public static class GameManager 
{
    public static int languageMenuScene = 0;
    public static int narratorOptionsScene = 1;
    public static int loginMenuScene = 2;
    public static int loadingScene = 3;
    public static int mainMenuScene = 4;
    public static int menuOptionsScene = 5;
    public static int warnControlsScene = 6;
    public static int matchMakingScene = 7;
    public static int waitMenuScene = 8;
    public static int tutorialMapScene = 9;
    public static int randomMapScene = 10;
    public static int winScene = 11;
    public static int deathScene = 12;

    public static string roomNum = "";
    public static string id = "";

    public static int lang; // 0 - ENG; 1 - PT
    public static bool alternate = true;

    public static bool alreadyLoaded;
    public static bool warned;
    public static bool narrator = true;

    public static bool inOptions;
    public static bool tutorial;
    public static bool pilot = true;

    public static bool deathCause;

    public static bool uranium;

    public static bool connError;

    public static void Save()
    {
        //StreamWriter writer = new StreamWriter("Assets/Resources/save.txt", false);
        //writer.WriteLine(uranium + "," + crab);
        //writer.Close();
    }

    public static void Load()
    {
        //StreamReader reader = new StreamReader("Assets/Resources/save.txt");
        //string[] loaded = Regex.Split((reader.ReadToEnd()), ",");
        //reader.Close();

        //uranium = Convert.ToBoolean(loaded[0]);
        //crab = Convert.ToBoolean(loaded[1]);

        //alreadyLoaded = true;
    }
}
