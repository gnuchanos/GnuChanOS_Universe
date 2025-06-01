using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks {



    void Start() {
        Debug.Log("Connecting....!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log("Joined lobby successfully.");
        // You can now create or join rooms.
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarning($"Disconnected from Photon. Reason: {cause}");
    }
}