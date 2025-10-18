using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    // 👉 ini variabel yang dibaca UIButtonHandler
    public static string sceneToLoad;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Delay kecil supaya animasi loading muncul
        yield return new WaitForSeconds(1f);

        // Mulai load scene target secara async
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Bisa tambahkan progress bar di sini (optional)
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
