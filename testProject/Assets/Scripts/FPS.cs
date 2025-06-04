using UnityEngine;
using Photon.Pun;

public class FPS : MonoBehaviour
{
    public Transform head;

    private CharacterController characterController;
    private float speed = 2;
    private float gravity = -9.81f;
    private Vector3 velocity;

    private float mouseSensitivity = 500f;
    private float xRotation = 0f;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();

        if (head == null)
        {
            Debug.LogError("Head (kamera boş objesi) atanmadı!");
        }

        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // Diğer oyuncular için kamerayı devre dışı bırak
            if (head != null && head.GetComponentInChildren<Camera>() != null)
            {
                head.GetComponentInChildren<Camera>().enabled = false;
                AudioListener listener = head.GetComponentInChildren<AudioListener>();
                if (listener != null) listener.enabled = false;
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}