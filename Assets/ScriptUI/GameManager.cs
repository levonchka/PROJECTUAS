using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;

    // Panggil fungsi ini kalau player mati
    public void PlayerDie()
    {
        if (isGameOver) return;
        isGameOver = true;
        SceneManager.LoadScene("Lose");
    }

    // Panggil fungsi ini kalau player sampai gate
    public void PlayerWin()
    {
        if (isGameOver) return;
        isGameOver = true;
        SceneManager.LoadScene("Win");
    }
}
