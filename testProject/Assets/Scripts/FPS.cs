using UnityEngine;
using Photon.Pun;

public class FPS : MonoBehaviour {
    public Transform head;
    public GameObject flyCamPrefab;

    private CharacterController characterController;
    private PhotonView photonView;

    private float speed = 2f;
    private float gravity = -9.81f;
    private Vector3 velocity;

    private float mouseSensitivity = 500f;
    private float xRotation = 0f;

    public bool flyCamMode = false;
    public bool CameraIsMoving = false;

    private GameObject flyCam;

    void Start() {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();

        if (head == null) Debug.LogError("Head atanmadı!");

        if (photonView.IsMine) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            if (head != null && head.GetComponentInChildren<Camera>() != null) {
                head.GetComponentInChildren<Camera>().enabled = false;
                AudioListener listener = head.GetComponentInChildren<AudioListener>();
                if (listener != null) {
                    listener.enabled = false;
                }
            }
        }
    }

    void Update() {
        if (!photonView.IsMine) return;

        // F: FlyCam aç/kapat
        if (Input.GetKeyDown(KeyCode.F)) {
            if (!flyCamMode) {
                flyCam = Instantiate(flyCamPrefab, head.position + head.forward * 2f + Vector3.up, Quaternion.identity);
                flyCamMode = true;
                CameraIsMoving = true;
                Cursor.lockState = CursorLockMode.Locked;
                xRotation = 0f;
            } else {
                Destroy(flyCam);
                flyCamMode = false;
                CameraIsMoving = false;
                Cursor.lockState = CursorLockMode.Locked;
                xRotation = 0f;
            }
        }

        if (flyCamMode) {
            // V: Kamera sabit (tank mode) ↔ kamera serbest (flycam)
            if (Input.GetKeyDown(KeyCode.V)) {
                if (!CameraIsMoving) {
                    CameraIsMoving = true;
                } else {
                    CameraIsMoving = false;
                }
            }
        }


        if (flyCamMode) {
            if (CameraIsMoving) {
                HandleFlyCam();
            } else {
                HandleTankControl();
            }
        } else {
            HandleNormalFPS();
        }
    }

    void HandleNormalFPS() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 movement = move * speed;

        velocity.y += gravity * Time.deltaTime;
        movement.y = velocity.y;

        characterController.Move(movement * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        Debug.Log(mouseY);

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleFlyCam() {
        if (flyCam == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0;

        if (Input.GetKey(KeyCode.Space)) y = 1;
        if (Input.GetKey(KeyCode.LeftControl)) y = -1;

        Vector3 moveDir = (flyCam.transform.forward * z + flyCam.transform.right * x + Vector3.up * y).normalized;
        flyCam.transform.position += moveDir * speed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        float yaw = flyCam.transform.eulerAngles.y + mouseX;

        flyCam.transform.rotation = Quaternion.Euler(xRotation, yaw, 0f);
    }

    void HandleTankControl() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
}