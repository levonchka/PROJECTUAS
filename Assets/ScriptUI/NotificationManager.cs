using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI messageText;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            Debug.LogWarning("⚠️ MessageText belum diisi di NotificationManager!");
            return;
        }

        Debug.Log("Showing message: " + message); // tambahkan ini

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        messageText.gameObject.SetActive(false);
    }
}
