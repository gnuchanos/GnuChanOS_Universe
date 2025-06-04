using UnityEngine;
using Photon.Pun;

public class bodyChange : MonoBehaviourPun
{
    public GameObject body0;
    public GameObject body1;

    void Start()
    {
        SetBody(0);
    }

    void Update()
    {
        if (!photonView.IsMine) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            photonView.RPC("SetBody", RpcTarget.AllBuffered, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            photonView.RPC("SetBody", RpcTarget.AllBuffered, 1);
        }
    }

    [PunRPC]
    void SetBody(int index)
    {
        body0.SetActive(index == 0);
        body1.SetActive(index == 1);
    }
}