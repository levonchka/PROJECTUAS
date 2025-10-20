using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    [Header("Optional UI Feedback")]
    public NotificationManager notificationManager; // untuk tampilkan pesan di layar (jika ada)

    private const string PreviousSceneKey = "PreviousScene"; // key penyimpanan scene sebelumnya

    // --- Tombol Navigasi ---
    public void LoadMainMenu() => LoadWithLoadingScene("Title");
    public void RestartGame() => LoadWithLoadingScene("Trial");
    public void LoadHowToPlay() => LoadWithLoadingScene("How to Play");
    public void LoadSettings() => LoadWithLoadingScene("Settings");
    public void LoadInGameMenu() => LoadWithLoadingScene("InGameMenu");

    // ✅ Tombol Resume (langsung balik ke game sebelumnya)
    public void ResumeGame()
    {
        if (PlayerPrefs.HasKey(PreviousSceneKey))
        {
            string previousScene = PlayerPrefs.GetString(PreviousSceneKey);
            Debug.Log($"[UIButtonHandler] Resuming game → Back to {previousScene}");

            // Langsung load tanpa loading screen agar cepat
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("[UIButtonHandler] No previous scene found! Loading Trial as fallback.");
            SceneManager.LoadScene("Trial");
        }
    }

    // ✅ Tombol Back ke scene sebelumnya (pakai LoadingScene)
    public void GoBack()
    {
        if (PlayerPrefs.HasKey(PreviousSceneKey))
        {
            string previousScene = PlayerPrefs.GetString(PreviousSceneKey);
            Debug.Log($"[UIButtonHandler] Going back to previous scene: {previousScene}");
            LoadWithLoadingScene(previousScene);
        }
        else
        {
            Debug.LogWarning("[UIButtonHandler] No previous scene found! Returning to Title.");
            LoadWithLoadingScene("Title");
        }
    }

    // --- Quit ---
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("[UIButtonHandler] Game Closed.");
    }

    // --- Internal Scene Loader ---
    private void LoadWithLoadingScene(string targetScene)
    {
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogWarning("[UIButtonHandler] Target scene name is empty!");
            return;
        }

        // Reset Time.timeScale sebelum pindah scene
        Time.timeScale = 1f;

        // Simpan scene saat ini sebagai previous scene
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != targetScene)
        {
            PlayerPrefs.SetString(PreviousSceneKey, currentScene);
            PlayerPrefs.Save();
            Debug.Log($"[UIButtonHandler] Saved previous scene: {currentScene}");
        }

        // Pindah ke scene loading
        LoadingScene.sceneToLoad = targetScene;
        SceneManager.LoadScene("Loading");
        Debug.Log($"[UIButtonHandler] Loading scene: {targetScene}");
    }

    // --- Apply Settings ---
    public void ApplySettings()
    {
        SettingsManager settings = FindObjectOfType<SettingsManager>();
        if (settings != null)
        {
            settings.ApplySettings();
            ShowNotification(" Settings Applied!");
            Debug.Log("[UIButtonHandler] Settings applied & saved.");
        }
        else
        {
            Debug.LogWarning("[UIButtonHandler] No SettingsManager found in scene!");
        }
    }

    // --- Reset Settings ---
    public void ResetSettings()
    {
        SettingsManager settings = FindObjectOfType<SettingsManager>();
        if (settings != null)
        {
            settings.ResetUI();
            ShowNotification(" Settings Reset to Default!");
            Debug.Log("[UIButtonHandler] Settings reset to default.");
        }
        else
        {
            Debug.LogWarning("[UIButtonHandler] No SettingsManager found in scene!");
        }
    }

    // --- Notifikasi opsional ---
    private void ShowNotification(string message)
    {
        if (notificationManager != null)
            notificationManager.ShowMessage(message);
        else
            Debug.Log($"[UIButtonHandler] Notification: {message}");
    }
}
