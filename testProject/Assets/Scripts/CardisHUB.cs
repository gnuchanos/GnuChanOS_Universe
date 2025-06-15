using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Photon.Pun;


[RequireComponent(typeof(AudioSource))]
public class CardisHUB : MonoBehaviourPun {
    public AudioClip InSound;
    private AudioSource audioSource;

    public GameObject R0;
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;

    public GameObject EnegySpiner;

    private float r0r;
    private float r1r;
    private float r2r;
    private float r3r;
    private float timer;

    void Start() {
        r0r = Random.Range(-5f, 5f);
        r1r = Random.Range(-5f, 5f);
        r2r = Random.Range(-5f, 5f);
        r3r = Random.Range(-5f, 5f);
        timer = Random.Range(1, 5);

        // Sounds
        audioSource = GetComponent<AudioSource>();
        if (InSound != null) {
            audioSource.PlayOneShot(InSound);
        }
    }



    void PlayInSound() {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(InSound);
        }
    }


    public Material targetMaterial;
    public string fileName = "screen.png";
    public Texture2D mainTexture;
    private bool MainTextureIsRender = true;


    void ScreenChange() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (MainTextureIsRender) {
                string basePath = Application.dataPath; // Assets dizini
                string fullPath = Path.Combine(basePath, "../" + fileName); // Çalışan EXE'nin yanına bak

                fullPath = Path.GetFullPath(fullPath); // Yolun düzgünleşmesini sağla

                if (File.Exists(fullPath)) {
                    byte[] fileData = File.ReadAllBytes(fullPath);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(fileData);

                    targetMaterial.mainTexture = tex;
                    MainTextureIsRender = false;

                    // Emission texture olarak da ata
                    targetMaterial.SetTexture("_EmissionMap", tex);
                    targetMaterial.EnableKeyword("_EMISSION");

                    Debug.Log("Texture yüklendi: " + fullPath);
                } else {
                    Debug.LogWarning("Texture dosyası bulunamadı: " + fullPath);
                }
            } else {
                    targetMaterial.mainTexture = mainTexture;
                    MainTextureIsRender = true;

                    // Emission texture olarak da ata
                    targetMaterial.SetTexture("_EmissionMap", mainTexture);
                    targetMaterial.EnableKeyword("_EMISSION");
            }

        }
    }

    void Update() {
        if (PhotonNetwork.IsMasterClient) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timer = Random.Range(1, 5);
                r0r = Random.Range(-5f, 5f);
                r1r = Random.Range(-5f, 5f);
                r2r = Random.Range(-5f, 5f);
                r3r = Random.Range(-5f, 5f);
            }

            float r0 = r0r * Time.deltaTime;
            float r1 = r1r * Time.deltaTime;
            float r2 = r2r * Time.deltaTime;
            float r3 = r3r * Time.deltaTime;

            R0.transform.Rotate(0f, 0f, r0);
            R1.transform.Rotate(0f, 0f, r1);
            R2.transform.Rotate(0f, 0f, r2);
            R3.transform.Rotate(0f, 0f, r3);

            EnegySpiner.transform.Rotate(0f, 0f, 50 * Time.deltaTime);


            ScreenChange();
            PlayInSound();
        }
    }




}