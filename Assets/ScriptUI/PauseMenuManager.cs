using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;     // panel pause menu, child dari Canvas
    public GameObject defaultButton;      // tombol default dipilih saat pause

    [Header("Gameplay References")]
    public MonoBehaviour[] scriptsToDisableOnPause; // semua script gameplay yang menerima input

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // pastikan tertutup saat mulai
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel == null) return;
        StartCoroutine(ActivatePauseMenu());
    }

    private IEnumerator ActivatePauseMenu()
    {
        // Aktifkan panel pause
        pauseMenuPanel.SetActive(true);

        // Tunggu 2 frame supaya EventSystem siap
        yield return null;
        yield return null;

        // Pilih tombol default agar ESC/klik pertama langsung diterima
        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // reset selection
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        // Disable semua gameplay scripts
        foreach (var script in scriptsToDisableOnPause)
        {
            if (script != null)
                script.enabled = false;
        }

        isPaused = true;
        Debug.Log("[PauseMenu] Game Paused (ESC & klik pertama langsung responsif)");
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel == null) return;

        pauseMenuPanel.SetActive(false);

        // Enable kembali semua gameplay scripts
        foreach (var script in scriptsToDisableOnPause)
        {
            if (script != null)
                script.enabled = true;
        }

        isPaused = false;
        Debug.Log("[PauseMenu] Game Resumed");
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSettings()
    {
        ResumeGame();
        SceneManager.LoadScene("Settings");
    }

    public void LoadMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("[PauseMenu] Game Closed.");
    }

    // Getter untuk isPaused, bisa dipakai di script lain
    public bool IsPaused()
    {
        return isPaused;
    }
}
