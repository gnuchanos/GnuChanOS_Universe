using UnityEngine;
using Photon.Pun;

public class PlaySoundPlayer : MonoBehaviourPun {
    public FPS fpsScript;

    public AudioClip WalkSound;
    [Range(0f, 1f)]
    public float volume = 0.2f;

    private AudioSource audioSource;
    private bool isWalking = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = volume;
        audioSource.loop = true;
    }

    void Update() {
        if (photonView.IsMine) {
            // Sadece kendi karakterimizde input kontrolü yapıyoruz
            bool walkingNow = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

            if (walkingNow != isWalking) {
                if (!fpsScript.CameraIsMoving) {
                    isWalking = walkingNow;
                    photonView.RPC("RPC_PlayWalkSound", RpcTarget.All, isWalking);
                }
            }
        }
    }

    [PunRPC]
    void RPC_PlayWalkSound(bool play) {
        if (play) {
            if (!audioSource.isPlaying) {
                audioSource.clip = WalkSound;
                audioSource.Play();
            }
        } else {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
        }
    }
}