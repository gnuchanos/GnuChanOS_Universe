using UnityEngine;
using Photon.Pun;

public class Maps : MonoBehaviourPun {
    public GameObject CardisMap;

    void Update()
    {
        if (!photonView.IsMine) {
            return;
        }

        // if (Input.GetKeyDown(KeyCode.F1)) {
        //     photonView.RPC("SetMap", RpcTarget.AllBuffered, 0);
        // }

    }

    [PunRPC]
    void SetMap(int mapIndex) {
        CardisMap.SetActive(mapIndex == 0);
    }
}