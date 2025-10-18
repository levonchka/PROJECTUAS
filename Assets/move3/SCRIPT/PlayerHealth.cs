using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar; // referensi ke script HealthBar

    private Animator anim;
    private bool isDead = false;
    private GameManager gameManager; // referensi ke GameManager

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        gameManager = FindObjectOfType<GameManager>(); // cari otomatis di scene

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        Debug.Log($"Player kena damage: {damage}, sisa darah: {currentHealth}");

        if (currentHealth > 0)
        {
            if (anim != null)
                anim.SetTrigger("isHit");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player mati!");

        if (anim != null)
            anim.SetTrigger("isDead");

        // Matikan collider biar gak bisa diserang lagi
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        // 🔥 Panggil Lose Scene
        if (gameManager != null)
        {
            // kasih sedikit delay supaya animasi death selesai dulu
            Invoke(nameof(GoToLoseScene), 2.5f);
        }
        else
        {
            Debug.LogWarning("GameManager tidak ditemukan, tidak bisa pindah ke Lose scene!");
        }
    }

    void GoToLoseScene()
    {
        gameManager.PlayerDie();
    }
}
