using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{name} menerima {damage} damage. Sisa HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            PlayHitEffect();
        }
    }

    void PlayHitEffect()
    {
        anim.ResetTrigger("isHit");
        anim.SetTrigger("isHit");
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        currentHealth = 0;
        anim.SetBool("isDead", true);
        anim.ResetTrigger("isHit");
        anim.CrossFade("Die", 0.1f);

        // Matikan collider & AI
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Hancurkan setelah 3 detik
        Destroy(gameObject, 3f);
        Debug.Log($"{name} telah mati ☠️");
    }
}
