using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;  // Sahne yönetimi için

public class CreateRoom : MonoBehaviourPunCallbacks
{
    private string defaultPrefabName = "fps"; // prefab adı dosyadaki haliyle
    private string roomName = "AnaOda"; // sabit oda adı
    private string sceneName = "mainGameHUB"; // yüklenecek sahne adı

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Sunucuya bağlanıldı. Odaya katılınıyor...");

        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("playerPrefab"))
        {
            Hashtable props = new Hashtable { { "playerPrefab", defaultPrefabName } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Odaya katılamadı: " + message + " Yeni oda oluşturulacak.");

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya girildi: " + PhotonNetwork.CurrentRoom.Name);

        if (IsSceneInBuildSettings(sceneName))
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            Debug.LogError($"Sahne '{sceneName}' Build Settings'e ekli değil! Lütfen ekleyin.");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Oda oluşturulamadı: " + message);
    }

    // Build Settings'te sahne var mı diye kontrol eden fonksiyon
    bool IsSceneInBuildSettings(string scene)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneNameFromPath == scene)
                return true;
        }
        return false;
    }
}