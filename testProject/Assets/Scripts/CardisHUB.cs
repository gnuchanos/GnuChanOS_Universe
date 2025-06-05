using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CardisHUB : MonoBehaviourPun {

    public GameObject R0;
    public GameObject R1;
    public GameObject R2;
    public GameObject R3;

    public GameObject EnegySpiner;

    public float rotationSpeed = 30f;

    void Update() {
        if (PhotonNetwork.IsMasterClient) {
            // Sadece MasterClient döndürür ve sonucu herkese yollar
            float delta = rotationSpeed * Time.deltaTime;

            R0.transform.Rotate(0f, 0f, delta);
            R1.transform.Rotate(0f, 0f, -delta);
            R2.transform.Rotate(0f, 0f, delta);
            R3.transform.Rotate(0f, 0f, -delta);

            EnegySpiner.transform.Rotate(0f, 0f, -delta);
        }
    }




}