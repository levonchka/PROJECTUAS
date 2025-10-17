using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

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

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player mati!");

        if (anim != null)
            anim.SetTrigger("isDead");

        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;
    }
}
