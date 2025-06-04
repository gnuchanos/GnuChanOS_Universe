using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class defaultSkin_BottomBodyAnim : MonoBehaviourPun, IPunObservable {
    public Animator TopBodyAnim;

    bool forward = false;
    bool backward = false;


    void Update() {
        if (photonView.IsMine) {
            if (Input.GetKey(KeyCode.W)) {
                forward = true;
                backward = false;
            } else if (Input.GetKey(KeyCode.S)) {
                forward = false;
                backward = true;
            } else {
                forward = false;
                backward = false;
            }

            TopBodyAnim.SetBool("forward", forward);
            TopBodyAnim.SetBool("backward", backward);

        } else {
            TopBodyAnim.SetBool("forward", forward);
            TopBodyAnim.SetBool("backward", backward);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(forward);
            stream.SendNext(backward);
        } else {
            forward = (bool)stream.ReceiveNext();
            backward = (bool)stream.ReceiveNext();
        }
    }
}