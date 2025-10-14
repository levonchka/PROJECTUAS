using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // drag slider UI dari canvas

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar) healthBar.maxValue = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (healthBar) healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player mati!");
        }
    }

    // contoh player attack (bisa ganti trigger dari input)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // cari enemy di sekitar
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemyHealth>().TakeDamage(15);
            }
        }
    }
}
