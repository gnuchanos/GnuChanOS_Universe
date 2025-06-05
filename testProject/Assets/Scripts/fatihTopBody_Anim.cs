using UnityEngine;
using Photon.Pun;

public class fatihTopBody_Anim : MonoBehaviourPun, IPunObservable {

    public Animator TopBodyAnim;

    bool forward = false;
    bool backward = false;

    void Update() {
        if (photonView.IsMine) {
            forward = Input.GetKey(KeyCode.W);
            backward = Input.GetKey(KeyCode.S);

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

            TopBodyAnim.SetBool("forward", forward);
            TopBodyAnim.SetBool("backward", backward);
        }
    }
}