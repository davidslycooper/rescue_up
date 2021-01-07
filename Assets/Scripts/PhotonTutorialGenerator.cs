using Photon.Pun;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PhotonTutorialGenerator: MonoBehaviour
{
    PhotonView PV;

    TutorialSteps steps;

    MapGenerator mapGen;
    MeshGenerator meshGen;

    Mesh mesh;
    MeshFilter caveMesh;
    GameObject cave;
    public GameObject cavePrefab;
    public GameObject pause;

    [Range(0, 100)]
    public int mineralPercent;

    [Range(0, 10)]
    public int enemyNumber;

    public int[,] map;
    public List<float[]> rocks;
    public List<float[]> corals;

    Vector3 startPoint;

    PlayerLog log;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        steps = GetComponent<TutorialSteps>();

        mapGen = GetComponent<MapGenerator>();
        meshGen = GetComponent<MeshGenerator>();

        cave = Instantiate(cavePrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        caveMesh = GetComponentInChildren<MeshFilter>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        if (GameManager.pilot)
        {
            map = mapGen.GenerateMap(20,30);
            startPoint = mapGen.startPoint;

            mesh = meshGen.GenerateMesh(map, 1);
            meshGen.Generate2DColliders();
            caveMesh.mesh = mesh;

            List<List<float[]>> spawns = meshGen.GetSpawns(startPoint, mineralPercent, enemyNumber);

            rocks = spawns[0];
            corals = spawns[2];

            log.StartTutorial();

            GetComponent<EntityManager>().Spawn(startPoint: startPoint, rocks: rocks, corals: corals);

            if (PhotonNetwork.InRoom)
            {
                PV.RPC("RPC_Start", RpcTarget.Others);
            }

            pause.SetActive(true);

            steps.StartTutorial();
        } 
        else if (!PhotonNetwork.InRoom)
        {
            map = mapGen.GenerateMap(20, 30);
            startPoint = mapGen.startPoint;

            mesh = meshGen.GenerateMesh(map, 1);
            meshGen.Generate2DColliders();
            caveMesh.mesh = mesh;

            log.StartTutorial();

            GetComponent<EntityManager>().Spawn(startPoint);

            pause.SetActive(true);

            steps.StartTutorial();
        }
    }

    [PunRPC]
    public void RPC_Start()
    {
        map = mapGen.GenerateMap(20, 30);
        startPoint = mapGen.startPoint;

        mesh = meshGen.GenerateMesh(map, 1);
        meshGen.Generate2DColliders();
        caveMesh.mesh = mesh;

        log.StartTutorial();

        GetComponent<EntityManager>().Spawn(startPoint);       

        pause.SetActive(true);

        steps.StartTutorial();
    }
}
