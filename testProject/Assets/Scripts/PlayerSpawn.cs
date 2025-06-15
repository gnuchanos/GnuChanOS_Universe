using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    public GameObject fpsPrefab;
    public Transform spawnPoint;

    private bool isInRoom = false;
    private bool sceneLoaded = false;

    new void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    new void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start() {
        isInRoom = PhotonNetwork.InRoom;
        if (isInRoom) {
            Debug.Log("Başlangıçta zaten odadayız.");
            TrySpawnPlayer();
        }
    }

    public override void OnJoinedRoom() {
        Debug.Log("PlayerSpawn: Odaya girildi.");
        isInRoom = true;

        TrySpawnPlayer();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("PlayerSpawn: Sahne yüklendi: " + scene.name);
        if (scene.name == "mainGameHUB") {
            sceneLoaded = true;
            TrySpawnPlayer();
        }
    }

    void TrySpawnPlayer() {
        if (isInRoom && sceneLoaded) {
            if (fpsPrefab == null) {
                Debug.LogError("fpsPrefab is null");
                return;
            }
            if (spawnPoint == null) {
                Debug.LogError("spawnPoint is null!");
                return;
            }

            Debug.Log("PlayerSpawn: player Spawning!");
            GameObject player = PhotonNetwork.Instantiate(fpsPrefab.name, spawnPoint.position, Quaternion.identity);
            player.name = "Player_" + Random.Range(1000, 9999);
            Debug.Log("PlayerSpawn: Here we gooooo: " + player.name);
        } else {
            Debug.Log("PlayerSpawn: room is not ready");
        }
    }
}