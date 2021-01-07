using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EntityManager : MonoBehaviour
{
    PhotonView PV;

    public GameObject rockPrefab;
    public GameObject[] mineralPrefabs;
    public GameObject coralPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject brokenRockPrefab;
    public GameObject treasurePrefab;
    public GameObject surfacePrefab;

    GameObject sub;
    SubManager subMan;
    GameObject panel;

    List<GameObject> mineralObjects;
    GameObject treasureObject;

    int enemyNum;

    public int caught;

    PlayerLog log;

    public void Spawn(Vector3 startPoint, List<float[]> rocks = null, List<float[]> minerals = null, List<float[]> corals = null, List<float[]> enemies = null,
        List<float[]> brokenRocks = null, List<float[]> treasure = null)
    {
        rocks = rocks ?? new List<float[]>();
        minerals = minerals ?? new List<float[]>();
        corals = corals ?? new List<float[]>();
        enemies = enemies ?? new List<float[]>();
        brokenRocks = brokenRocks ?? new List<float[]>();
        treasure = treasure ?? new List<float[]>();

        PV = GetComponent<PhotonView>();

        log = GameObject.Find("PlayerLog").GetComponent<PlayerLog>();

        GetComponent<PhotonPlayer>().SpawnPlayers(startPoint - new Vector3(0, 6, 0));

        sub = GetComponent<PhotonPlayer>().GetSub();

        mineralObjects = new List<GameObject>();

        if (GameManager.pilot)
        {
            subMan = sub.GetComponent<SubManager>();
            sub.GetComponent<SubLightManager>().SetStartPoint(startPoint);

            SpawnRocks(rocks);
            SpawnCorals(corals);
            SpawnBrokenRocks(brokenRocks);
            SpawnEnemies(enemies);
            SpawnSurface(startPoint);
        }

        int[] mineralCount = SpawnMinerals(minerals);

        SpawnTreasure(treasure);

        Debug.Log("Minerais: " + minerals.Count.ToString()
                + " (Carvão: " + mineralCount[0].ToString()
                + "; Urânio: " + mineralCount[1].ToString()
                + "; Quartzo: " + mineralCount[2].ToString()
                + "; Âmbar: " + mineralCount[3].ToString()
                + "); Inimigos: " + enemies.Count.ToString()
                + "; Corais: " + corals.Count.ToString()
                + "; Passagens Bloqueadas: " + brokenRocks.Count.ToString());
    }

    public void SpawnRocks(List<float[]> rocks)
    {
        foreach (float[] rock in rocks)
        {
            Instantiate(rockPrefab, new Vector3(rock[0], rock[1]), Quaternion.Euler(new Vector3(0, 0, 0)), transform);
        }
    }

    public void SpawnCorals(List<float[]> corals)
    {
        foreach (float[] coral in corals)
        {
            Instantiate(coralPrefab, new Vector3(coral[0], coral[1]), Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 3) * 90)), transform);
        }
    }

    public void SpawnBrokenRocks(List<float[]> brokenRocks)
    {
        foreach (float[] brokenRock in brokenRocks)
        {
            Vector3 pos = new Vector3(brokenRock[0], brokenRock[1]);

            GameObject rock = Instantiate(brokenRockPrefab, pos, Quaternion.Euler(new Vector3(0, 0, 0)), transform);

            if (!GameManager.tutorial) 
            {
                log.SpawnBrokenRock(pos);
            }
        }
    }

    public void SpawnEnemies(List<float[]> enemies)
    {
        foreach (float[] enemy in enemies)
        {
            Vector3 pos = new Vector3(enemy[0], enemy[1]);

            GameObject newEnemy = PhotonNetwork.Instantiate(Path.Combine("prefabs", "enemies", (enemy[2] == 0) ? "pirana" : "lizz"), pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            newEnemy.transform.SetParent(transform);
            newEnemy.GetComponent<EnemyKill>().SetId(enemyNum);
            newEnemy.GetComponent<EnemyBehavior>().SetId(enemyNum);

            if (!GameManager.tutorial)
            {
                log.SpawnEnemy(enemyNum, pos);
            }

            enemyNum++;
        }
    }

    public int[] SpawnMinerals(List<float[]> minerals)
    {
        int[] mineralCount = new int[4];

        for (int i = 0; i < minerals.Count; i++)
        {
            GameObject mineral = Instantiate(mineralPrefabs[(int)minerals[i][2]], new Vector3(minerals[i][0], minerals[i][1]), Quaternion.Euler(new Vector3(0, 0, 0)), transform);
            if (mineral.GetComponent<Caught>() != null)
            {
                mineral.GetComponent<Caught>().SetMineralIndex(mineralObjects.Count);
            }
            mineralObjects.Add(mineral);

            int type = (int)minerals[i][2];

            switch (type)
            {
                case 0:
                    mineralCount[0]++;
                    if (!GameManager.tutorial)
                    {
                        log.SpawnCoal();
                    }
                    break;
                case 1:
                    mineralCount[1]++;
                    if (!GameManager.tutorial)
                    {
                        log.SpawnUranium();
                    }
                    break;
                case 2:
                    mineralCount[2]++;
                    if (!GameManager.tutorial)
                    {
                        log.SpawnQuartz();
                    }
                    break;
                case 3:
                    mineralCount[3]++;
                    if (!GameManager.tutorial)
                    {
                        log.SpawnAmber();
                    }
                    break;
            }
        }
        return mineralCount;
    }

    public void SpawnTreasure(List<float[]> treasure)
    {
        if (treasure.Count > 0)
        {
            Vector3 pos = new Vector3(treasure[0][0], treasure[0][1]);

            if ((treasure.Count > 0))
            {
                treasureObject = Instantiate(treasurePrefab, pos, Quaternion.Euler(new Vector3(0, 0, 0)), transform);
            }

            if (!GameManager.tutorial)
            {
                log.SpawnTreasure(pos);
            }
        }
    }

    public void SpawnSurface(Vector3 startPoint)
    {
        Instantiate(surfacePrefab, startPoint, Quaternion.Euler(new Vector3(0, 0, 0)));

        if (!GameManager.tutorial)
        {
            log.SpawnSurface(startPoint);
        }
    }

    public void CatchMineral(int mineralIndex, int mineralType)
    {
        log.Catch(mineralIndex, mineralType);

        sub.GetComponent<SubManager>().CatchSound();

        if (mineralType == 4)
        {
            caught++;
            subMan.CatchTreasure();
            DestroyTreasure();
        }
        else
        {
            if (mineralType == 1 && !GameManager.uranium)
            {
                GameObject.FindWithTag("Helper").GetComponent<Helper>().ExplainUranium();
            }
            caught++;
            Destroy(mineralObjects[mineralIndex]);
        }
        if (PhotonNetwork.InRoom)
        {
            PV.RPC("RPC_CatchMineral", RpcTarget.Others, mineralIndex, mineralType);
        }
    }

    public void DestroyTreasure()
    {
        Destroy(treasureObject);
    }

    [PunRPC]
    public void RPC_CatchMineral(int mineralIndex, int mineralType)
    {
        panel = GetComponent<PhotonPlayer>().GetPanel();

        if (mineralType == 4)
        {
            caught++;
            Destroy(treasureObject);
            panel.GetComponent<CraftingPanel>().CatchMineral(mineralType);
        }
        else
        {
            caught++;
            Destroy(mineralObjects[mineralIndex]);
            panel.GetComponent<CraftingPanel>().CatchMineral(mineralType);
        }
    }
}
