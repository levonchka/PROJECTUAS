using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill; // assign di inspector

    public void UpdateHealth(float current, float max)
    {
        healthBarFill.fillAmount = current / max;
    }

    // Opsional: biar healthbar selalu menghadap kamera
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
