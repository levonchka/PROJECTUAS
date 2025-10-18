using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;  // drag player di Inspector
    public Vector3 offset = new Vector3(0, 2f, 0); // posisi di atas kepala

    void LateUpdate()
    {
        if (target == null) return;

        // Hanya ikuti posisi player + offset
        transform.position = target.position + offset;

        // Selalu menghadap kamera, tapi tidak ikut rotasi player
        if (Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            camPos.y = transform.position.y; // hanya rotasi horizontal
            transform.LookAt(camPos);
        }
    }
}
