using UnityEngine;
using Photon.Pun;

public class DefaultSkin_BottomBodyAnim : MonoBehaviourPun, IPunObservable {
    public Animator TopBodyAnim;

    bool forward = false;
    bool backward = false;

    void Update() {
        if (photonView.IsMine) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                forward = true;
            } else {
                forward = false;
            }

            backward = Input.GetKey(KeyCode.S);

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