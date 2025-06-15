using UnityEngine;
using Photon.Pun;

public class FPS : MonoBehaviour {
    public Transform head;

    private Camera flyCam;

    private CharacterController characterController;
    private PhotonView photonView;

    private float speed = 2f;
    private float gravity = -9.81f;
    private Vector3 velocity;

    private float mouseSensitivity = 500f;

    private float xRotation = 0f;
    private float yaw = 0f;
    private float pitch = 0f;

    public bool flyCamMode = false;
    public bool CameraIsMoving = false;

    void Start() {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();

        if (head == null) {
            Debug.LogError("Head atanmadı!");
            return;
        }

        if (photonView.IsMine) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            if (head != null) {
                Camera c = head.GetComponentInChildren<Camera>();
                if (c != null) c.enabled = false;
                AudioListener listener = head.GetComponentInChildren<AudioListener>();
                if (listener != null) listener.enabled = false;
            }
        }

        // Otomatik FlyCam oluşturma
        GameObject go = new GameObject("FlyCam_AutoCreated");
        flyCam = go.AddComponent<Camera>();
        flyCam.fieldOfView = 90;
        go.AddComponent<AudioListener>();

        flyCam.nearClipPlane = 0.1f;
        flyCam.farClipPlane = 1000f;

        go.SetActive(false);
    }

    private bool CursorShow = false;

    void HideCursor() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!CursorShow) {
                Cursor.lockState = CursorLockMode.None;
                CursorShow = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                CursorShow = false;
            }
        } 
    }


    void Update() {
        if (!photonView.IsMine) { return; }

        HideCursor();


        if (Input.GetKeyDown(KeyCode.F)) {
            flyCamMode = !flyCamMode;

            flyCam.gameObject.SetActive(flyCamMode);

            if (flyCamMode) {
                flyCam.transform.position = head.position + head.forward * 2f + Vector3.up;
                flyCam.transform.rotation = Quaternion.LookRotation(head.forward, Vector3.up);
                yaw = flyCam.transform.eulerAngles.y;
                pitch = flyCam.transform.eulerAngles.x;
            }

            CameraIsMoving = flyCamMode;
            Cursor.lockState = CursorLockMode.Locked;
            xRotation = 0f;
        }

        if (flyCamMode) {
            if (Input.GetKeyDown(KeyCode.V)) {
                CameraIsMoving = !CameraIsMoving;
            }

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
        Vector3 movement = move * (speed + 2);

        velocity.y += gravity * Time.deltaTime;
        movement.y = velocity.y;

        characterController.Move(movement * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.localEulerAngles = new Vector3(xRotation, 0f, 0f);
    }

    void HandleFlyCam() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0f;

        if (Input.GetKey(KeyCode.Space)) y = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) y = -1f;

        Vector3 moveDir = (flyCam.transform.forward * z + flyCam.transform.right * x + Vector3.up * y).normalized;
        flyCam.transform.position += moveDir * speed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        flyCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
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