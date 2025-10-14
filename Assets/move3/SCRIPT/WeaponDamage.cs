using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 20;
    public bool isEnemyWeapon = false; // true kalau milik musuh
    private bool canDealDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage) return;

        if (isEnemyWeapon && other.CompareTag("Player"))
        {
            HeroController hero = other.GetComponent<HeroController>();
            if (hero != null)
            {
                hero.TakeDamage(damage);
                Debug.Log($"💥 Musuh mengenai Hero, -{damage} HP");
            }
        }
        else if (!isEnemyWeapon && other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"⚔️ Pedang mengenai {enemy.name}, -{damage} HP");
            }
        }
    }

    public void EnableDamage() => canDealDamage = true;
    public void DisableDamage() => canDealDamage = false;
}
