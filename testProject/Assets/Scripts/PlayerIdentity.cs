using UnityEngine;
using Photon.Pun;

public class PlayerIdentity : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)
        {
            string randomName = "Player_" + Random.Range(1000, 9999);
            photonView.RPC("SetPlayerName", RpcTarget.AllBuffered, randomName);
        }
    }

    [PunRPC]
    void SetPlayerName(string newName)
    {
        gameObject.name = newName;
    }
}