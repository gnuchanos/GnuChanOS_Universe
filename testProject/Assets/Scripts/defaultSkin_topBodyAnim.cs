using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class DefaultSkin_TopBodyAnim : MonoBehaviourPun, IPunObservable {

    public FPS fpsScript;

    public Animator TopBodyAnim;

    bool forward = false;
    bool backward = false;

    void Update() {
        if (photonView.IsMine) {
            if (!fpsScript.CameraIsMoving) {
                forward = Input.GetKey(KeyCode.W);
                backward = Input.GetKey(KeyCode.S);

                TopBodyAnim.SetBool("forward", forward);
                TopBodyAnim.SetBool("backward", backward);
            }
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