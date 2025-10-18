using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Ganti scene langsung
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Dengan scene Loading (transisi)
    public void LoadSceneWithLoading(string targetScene)
    {
        PlayerPrefs.SetString("NextScene", targetScene);
        SceneManager.LoadScene("Loading");
    }

    public void ReloadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Game exited.");
        Application.Quit();
    }
}
