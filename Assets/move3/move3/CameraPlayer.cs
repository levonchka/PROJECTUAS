using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Target Hero")]
    [SerializeField] Transform posHero;

    [Header("Camera Offset (jarak default dari hero)")]
    [SerializeField] Vector3 offset = new Vector3(0, 3f, -6f);

    [Header("Follow & Smoothness")]
    [SerializeField] float followSpeed = 10f;

    [Header("Rotation Settings")]
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField, Range(0f, 80f)] float tiltMin = 10f;
    [SerializeField, Range(0f, 80f)] float tiltMax = 60f;

    [Header("Zoom Settings")]
    [SerializeField] float zoomSpeed = 2f;
    [SerializeField] float minZoom = 3f;
    [SerializeField] float maxZoom = 10f;

    [Header("Cursor Lock")]
    [SerializeField] bool lockCursor = false; // kamu bisa ubah di Inspector

    private float yaw = 0f;   // rotasi horizontal (Y)
    private float pitch = 30f; // rotasi vertikal (X)
    private float currentZoom;

    void Start()
    {
        if (posHero == null)
        {
            GameObject heroObj = GameObject.Find("hero");
            if (heroObj != null) posHero = heroObj.transform;
        }

        currentZoom = Mathf.Abs(offset.z);

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (posHero == null) return;

        // ======== ROTASI KAMERA DENGAN MOUSE TANPA KLIK ========
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, tiltMin, tiltMax);

        // ======== ZOOM DENGAN SCROLL WHEEL ========
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        // ======== POSISI & ROTASI KAMERA ========
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = posHero.position - (rotation * Vector3.forward * currentZoom) + Vector3.up * offset.y;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(posHero.position + Vector3.up * 1.5f);
    }
}
