using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;

public class PhotonMapGeneration : MonoBehaviour
{
    PhotonView PV;

    MapGenerator mapGen;
    MeshGenerator meshGen;

    Mesh mesh;
    MeshFilter caveMesh;
    GameObject cave;
    public GameObject cavePrefab;
    public GameObject pause;

    [Range(0, 100)]
    public int mineralPercent;

    [Range(1, 10)]
    public int enemyNumber;

    public int[,] map;
    public List<float[]> rocks;
    public List<float[]> minerals;
    public List<float[]> corals;
    public List<float[]> enemies;
    public List<float[]> blockedPassages;
    public List<float[]> treasure;

    Vector3 startPoint;

    PlayerLog log;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        mapGen = GetComponent<MapGenerator>();
        meshGen = GetComponent<MeshGenerator>();

        cave = Instantiate(cavePrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        caveMesh = GetComponentInChildren<MeshFilter>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        if (GameManager.pilot || !PhotonNetwork.InRoom)
        {
            map = mapGen.GenerateMap();
            startPoint = mapGen.startPoint;

            mesh = meshGen.GenerateMesh(map, 1);
            meshGen.Generate2DColliders();
            caveMesh.mesh = mesh;

            List<List<float[]>> spawns = meshGen.GetSpawns(startPoint, mineralPercent, enemyNumber);

            rocks = spawns[0];
            minerals = spawns[1];
            corals = spawns[2];
            enemies = spawns[3];
            blockedPassages = spawns[4];
            treasure = spawns[5];

            //List<List<int>> outlines = meshGen.outlines;

            string genMap = MapToString(map);

            string genMinerals = ParamToString(minerals);
            string genEnemies = ParamToString(enemies);
            string genTreasure = ParamToString(treasure);

            string genStartPoint = startPoint.x.ToString(CultureInfo.InvariantCulture.NumberFormat) + "/" + startPoint.y.ToString(CultureInfo.InvariantCulture.NumberFormat);

            if (PhotonNetwork.InRoom)
            {
                PV.RPC("RPC_SetMap", RpcTarget.Others, mapGen.seed, genMap, genMinerals, genEnemies, genTreasure, genStartPoint);
            }

            log.StartMission(mapGen.seed);

            GetComponent<EntityManager>().Spawn(startPoint, rocks, minerals, corals, enemies, blockedPassages, treasure);

            pause.SetActive(true);
        }
    }

    void ClientGenerateMesh()
    {
        mesh = meshGen.GenerateMesh(map, 1);
        meshGen.Generate2DColliders();
        caveMesh.mesh = mesh;
    }

    string MapToString(int[,] mapValues)
    {
        string s = "";

        for (int x = 0; x < mapValues.GetLength(0); x++)
        {
            for (int y = 0; y < mapValues.GetLength(1); y++)
            {
                s += mapValues[x,y].ToString(CultureInfo.InvariantCulture.NumberFormat) + ",";
            }
            s = s.Remove(s.Length - 1, 1);
            s += ";";
        }
        s = s.Remove(s.Length - 1, 1);
        return s;
    }

    int[,] StringToMap(string s)
    {
        string[] rows = s.Split(';');

        int[,] genMap = new int[rows.Length, rows[0].Split(',').Length];

        for (int x = 0; x < rows.Length; x++)
        {
            string[] values = rows[x].Split(',');

            for (int y = 0; y < values.Length; y++)
            {
                genMap[x, y] = int.Parse(values[y]);
            }
        }
        return genMap;
    }

    string ParamToString(List<float[]> param)
    {
        string s = "";

        foreach (float[] coord in param)
        {
            s += coord[0].ToString(CultureInfo.InvariantCulture.NumberFormat) + "/" + coord[1].ToString(CultureInfo.InvariantCulture.NumberFormat);
            if (coord.Length == 3)
            {
                s += "/" + coord[2].ToString(CultureInfo.InvariantCulture.NumberFormat); 
            }
            s += ";";
        }
        s = s.Remove(s.Length - 1, 1);
        return s;
    }

    List<float[]> StringToParam(string s)
    {
        List<float[]> param = new List<float[]>();

        string[] coords = s.Split(';');

        foreach (string coord in coords)
        {
            string[] values = coord.Split('/');

            float[] genValues = new float[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                genValues[i] = float.Parse(values[i], CultureInfo.InvariantCulture.NumberFormat); 
            }

            param.Add(genValues);
        }
        return param;
    }

    [PunRPC]
    public void RPC_SetMap(string seed, string genMap, string genMinerals, string genEnemies, string genTreasure, string genStartPoint)
    {
        map = StringToMap(genMap);
        minerals = StringToParam(genMinerals);
        enemies = StringToParam(genEnemies);
        treasure = StringToParam(genTreasure);

        string[] point = genStartPoint.Split('/');

        startPoint = new Vector3(float.Parse(point[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(point[1], CultureInfo.InvariantCulture.NumberFormat));

        ClientGenerateMesh();

        log.StartMission(seed);

        GetComponent<EntityManager>().Spawn(startPoint, minerals: minerals, enemies: enemies, treasure: treasure);

        pause.SetActive(true);
    }
}
