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
        Debug.Log($"{name} menerima damage {damage}, sisa HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            anim.SetBool("isDead", true);
            anim.CrossFade("Die", 0.1f);
            Debug.Log($"{name} mati ☠️");
        }
        else
        {
            anim.SetTrigger("isHit");
        }
    }
}
