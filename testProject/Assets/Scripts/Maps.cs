using UnityEngine;
using Photon.Pun;

public class Maps : MonoBehaviourPun
{
    public GameObject CardisMap;
    public GameObject MinecraftMap;

    void Update()
    {
        if (!photonView.IsMine) return; // Sadece kendi girişlerini dikkate al

        if (Input.GetKeyDown(KeyCode.F1))
        {
            photonView.RPC("SetMap", RpcTarget.AllBuffered, 0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            photonView.RPC("SetMap", RpcTarget.AllBuffered, 1);
        }
    }

    [PunRPC]
    void SetMap(int mapIndex)
    {
        CardisMap.SetActive(mapIndex == 0);
        MinecraftMap.SetActive(mapIndex == 1);
    }
}