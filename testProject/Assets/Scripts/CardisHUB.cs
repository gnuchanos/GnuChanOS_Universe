using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardisHUB : MonoBehaviourPun {

    public GameObject ring0;
    public GameObject ring1;
    public GameObject ring2;
    public GameObject ring3;

    public GameObject energySpinner;

    public float rotationSpeed = 30f;

    public Animator doorAnimation;

    private bool doorOpen = false;

    void Update() {
        if (PhotonNetwork.IsMasterClient) {
            float delta = rotationSpeed * Time.deltaTime;

            ring0.transform.Rotate(0f, 0f, delta);
            ring1.transform.Rotate(0f, 0f, -delta);
            ring2.transform.Rotate(0f, 0f, delta);
            ring3.transform.Rotate(0f, 0f, -delta);

            energySpinner.transform.Rotate(0f, 0f, -delta);

            if (Input.GetKeyUp(KeyCode.E)) {
                if (!doorOpen) { 
                    doorOpen = true;
                } else {
                    doorOpen = false;
                }
                photonView.RPC("SetDoorStateRPC", RpcTarget.All, doorOpen);
            }
            Debug.Log(doorOpen);
        }
    }

    [PunRPC]
    void SetDoorStateRPC(bool open) {
        if (doorAnimation != null) {
            doorAnimation.SetBool("doorOpen", open);
        }
    }
}