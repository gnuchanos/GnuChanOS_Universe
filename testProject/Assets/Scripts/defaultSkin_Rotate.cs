using UnityEngine;
using Photon.Pun;

public class BottomBodyRotate : MonoBehaviourPun, IPunObservable
{
    public Transform fpsRoot;
    public Transform bodyTransform;

    private Quaternion targetRotation;

    void Update()
    {
        if (photonView.IsMine)
        {
            Vector3 forward = fpsRoot.forward;
            Vector3 right = fpsRoot.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 targetDir = Vector3.zero;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
                targetDir = (forward - right).normalized;

            } else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) {
                targetDir = (forward + right).normalized;

            } else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
                targetDir = (forward + right).normalized;

            } else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) {
                targetDir = (forward - right).normalized;

            } else {
                if (Input.GetKey(KeyCode.W)) {
                    targetDir = forward;
                } else if (Input.GetKey(KeyCode.S)) {
                    targetDir = forward;
                } else if (Input.GetKey(KeyCode.A)) {
                    targetDir = -right;
                } else if (Input.GetKey(KeyCode.D)) {
                    targetDir = right;
                }
            }

            if (targetDir != Vector3.zero) {
                bodyTransform.rotation = Quaternion.LookRotation(targetDir, Vector3.up);
            }
        }
        else
        {
            // Uzaktaki diğer oyuncular için rotasyon yumuşak şekilde uygulanır
            bodyTransform.rotation = Quaternion.Lerp(
                bodyTransform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Kendi rotasyonunu gönder
            stream.SendNext(bodyTransform.rotation);
        }
        else
        {
            // Diğer oyuncudan gelen rotasyonu al
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}