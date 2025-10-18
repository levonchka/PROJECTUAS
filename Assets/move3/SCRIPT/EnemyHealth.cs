using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarPrefab; // drag prefab ke sini (optional di prefab)
    public Vector3 healthBarOffset = new Vector3(0, 2f, 0); // ✅ bisa diatur manual tinggi/posisinya
    private HealthBar healthBarInstance;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

        // 🔹 Auto spawn health bar
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(
                healthBarPrefab,
                transform.position + healthBarOffset,
                Quaternion.identity
            );

            healthBarInstance = hb.GetComponent<HealthBar>();

            // sambungkan health bar ke musuh ini
            if (healthBarInstance != null)
            {
                healthBarInstance.target = transform;
                healthBarInstance.offset = healthBarOffset; // ✅ kirim offset custom
                healthBarInstance.UpdateHealth(currentHealth, maxHealth);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"{name} menerima {damage} damage. Sisa HP: {currentHealth}");

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
        else
            PlayHitEffect();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        currentHealth = 0;

        if (anim != null)
        {
            anim.SetBool("isDead", true);
            anim.CrossFade("Die", 0.1f);
        }

        Debug.Log($"{name} mati ☠️");

        // Nonaktifkan collider & AI
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        // 🔥 Hapus health bar setelah mati
        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);

        // 🔥 Hapus musuh setelah animasi selesai
        float deathDuration = GetAnimationClipLength("Die");
        Destroy(gameObject, deathDuration + 0.5f);
    }

    void PlayHitEffect()
    {
        if (anim == null) return;
        anim.ResetTrigger("isHit");
        anim.SetTrigger("isHit");
    }

    float GetAnimationClipLength(string clipName)
    {
        if (anim == null || anim.runtimeAnimatorController == null)
            return 0f;

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 2f;
    }
}
