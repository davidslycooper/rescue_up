using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MenuGenerator : MonoBehaviour
{
    public GameObject[] prefabs;

    [Range(1, 10)]
    public int fill;

    public RectTransform[] buttons;

    public string[] sides;
    List<int>[] spawnSides;

    void Start()
    {
        GetSides();
        Generate();
    }

    void GetSides()
    {
        spawnSides = new List<int>[sides.Length];

        for (int i = 0; i < sides.Length; i++)
        {
            spawnSides[i] = sides[i].Split(',').Select(Int32.Parse).ToList();
        }
    }

    void Generate()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int qty = UnityEngine.Random.Range(1, fill);
            
            for (int j = 0; j < qty; j++)
            {
                int side = spawnSides[i][UnityEngine.Random.Range(0, spawnSides[i].Count)];
                Vector3 pos = new Vector3(buttons[i].localPosition.x, buttons[i].localPosition.y);
                GameObject obj = Instantiate(prefabs[Mathf.Min(UnityEngine.Random.Range(0, 4), 1)], Vector3.zero, Quaternion.identity, transform);

                switch (side)
                {
                    case 0:
                        pos += new Vector3(UnityEngine.Random.Range(-buttons[i].sizeDelta.x / 2, buttons[i].sizeDelta.x / 2), buttons[i].sizeDelta.y / 2 + 45f);
                        obj.GetComponent<ImageRandomizer>().Set(0);
                        break;
                    case 1:
                        pos += new Vector3(buttons[i].sizeDelta.x / 2 + 25f, UnityEngine.Random.Range(-buttons[i].sizeDelta.y / 2, buttons[i].sizeDelta.y / 2));
                        obj.GetComponent<ImageRandomizer>().Set(1);
                        spawnSides[i].Remove(side);
                        break;
                    case 2:
                        pos += new Vector3(UnityEngine.Random.Range(-buttons[i].sizeDelta.x / 2, buttons[i].sizeDelta.x / 2), -buttons[i].sizeDelta.y / 2 - 45f);
                        obj.GetComponent<ImageRandomizer>().Set(2);
                        break;
                    case 3:
                        pos += new Vector3(-buttons[i].sizeDelta.x / 2 - 25f, UnityEngine.Random.Range(-buttons[i].sizeDelta.y / 2, buttons[i].sizeDelta.y / 2));
                        obj.GetComponent<ImageRandomizer>().Set(3);
                        spawnSides[i].Remove(side);
                        break;
                }
                obj.GetComponent<RectTransform>().localPosition = new Vector2(pos.x, pos.y);

                if (spawnSides[i].Count == 0)
                {
                    break;
                }
            }
        }
    }
}
