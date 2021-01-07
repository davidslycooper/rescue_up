using Photon.Pun;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonPlayer : MonoBehaviour
{
    PhotonView PV;

    GameObject sub;
    GameObject panel;
    GameObject panelImage;

    public GameObject panelPrefab;
    public GameObject subPrefab;
    public GameObject panelImagePrefab;

    public void SpawnPlayers(Vector3 startPoint)
    {
        PV = GetComponent<PhotonView>();

        if (GameManager.pilot)
        {
            sub = PhotonNetwork.Instantiate(Path.Combine("prefabs", "sub"), startPoint, Quaternion.identity);
            sub.GetComponent<SubManager>().SetStartPoint(startPoint);
            if (PhotonNetwork.InRoom)
            {
                PV.RPC("RPC_CreatePanel", RpcTarget.Others, startPoint.x, startPoint.y);
            }
            Camera.main.GetComponent<CameraFollow>().Configure(sub.transform);
            panel = null;
        }
        else if (!PhotonNetwork.InRoom)
        {
            sub = Instantiate(subPrefab, startPoint, Quaternion.identity);
            sub.GetComponent<SubManager>().SetStartPoint(startPoint);
            panel = Instantiate(panelPrefab, startPoint, Quaternion.identity);
            panel.transform.SetParent(sub.transform);

            panelImage = Instantiate(panelImagePrefab, new Vector3(0, 500, 0), Quaternion.identity);
            Camera.main.GetComponent<CameraFollow>().Configure(panelImage.transform);
        }
    }

    public GameObject GetSub()
    {
        return sub;
    }

    public GameObject GetPanel()
    {
        return panel;
    }

    [PunRPC]
    public void RPC_CreatePanel(float startPointX, float startPointY)
    {
        sub = GameObject.FindWithTag("Player");
        sub.GetComponent<SubManager>().SetStartPoint(new Vector3(startPointX, startPointY));
        panel = Instantiate(panelPrefab, new Vector3(startPointX, startPointY), Quaternion.identity);
        panel.transform.SetParent(sub.transform);

        panelImage = Instantiate(panelImagePrefab, new Vector3(0, 500, 0), Quaternion.identity);

        Camera.main.GetComponent<CameraFollow>().Configure(panelImage.transform);
    }
}
