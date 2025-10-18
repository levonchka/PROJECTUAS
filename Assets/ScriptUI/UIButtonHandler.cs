using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    [Header("Optional UI Feedback")]
    public NotificationManager notificationManager; // untuk tampilkan pesan di layar

    public void LoadMainMenu() => LoadWithLoadingScene("Title");
    public void RestartGame() => LoadWithLoadingScene("Trial");
    public void LoadHowToPlay() => LoadWithLoadingScene("How to Play");
    public void LoadSettings() => LoadWithLoadingScene("Settings");
    public void Back() => LoadWithLoadingScene("Title");

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    private void LoadWithLoadingScene(string targetScene)
    {
        LoadingScene.sceneToLoad = targetScene;
        SceneManager.LoadScene("Loading");
    }

    // ✅ Apply Settings
    public void ApplySettings()
    {
        SettingsManager settings = FindObjectOfType<SettingsManager>();
        if (settings != null)
        {
            settings.ApplySettings(); // panggil fungsi dari SettingsManager
        }

        // Beri notifikasi ke layar
        if (notificationManager != null)
            notificationManager.ShowMessage("Settings Applied!");

        Debug.Log("Settings applied & saved!");
    }

    // 🔄 Reset Settings
    public void ResetSettings()
    {
        SettingsManager settings = FindObjectOfType<SettingsManager>();
        if (settings != null)
        {
            settings.ResetUI();
        }

        if (notificationManager != null)
            notificationManager.ShowMessage("Settings Reset to Default!");

        Debug.Log("Settings reset to default!");
    }
}
