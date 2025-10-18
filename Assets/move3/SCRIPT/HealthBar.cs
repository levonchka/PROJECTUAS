using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBarFill; // drag fill image dari slider atau image bar

    [Header("Follow Settings")]
    public Transform target; // drag player (hero)
    public Vector3 offset = new Vector3(0, 2f, 0); // posisi di atas kepala

    public void UpdateHealth(float current, float max)
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = current / max;
    }

    void LateUpdate()
    {
        // Healthbar tetap di atas kepala player
        if (target != null)
            transform.position = target.position + offset;

        // Healthbar selalu menghadap kamera
        if (Camera.main != null)
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
