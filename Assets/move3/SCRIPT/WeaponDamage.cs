using UnityEngine;
using System.Collections;

public class WeaponDamage : MonoBehaviour
{
    public float damage = 25f;
    public float hitRange = 1f;
    public LayerMask enemyLayer;

    private bool canDealDamage = false;
    private Transform attackPoint;
    private Coroutine disableRoutine;

    void Start()
    {
        attackPoint = transform;
    }

    void Update()
    {
        if (!canDealDamage) return;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, hitRange, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            EnemyHealth enemy = enemyCollider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"💥 Musuh {enemyCollider.name} terkena {damage} damage!");
            }
        }
    }

    public void EnableDamage()
    {
        canDealDamage = true;
        if (disableRoutine != null) StopCoroutine(disableRoutine);
        Debug.Log("🗡️ WeaponDamage ENABLED");
    }

    public void DisableDamage()
    {
        // Tambahkan delay sebelum benar-benar disable
        if (disableRoutine != null) StopCoroutine(disableRoutine);
        disableRoutine = StartCoroutine(DisableAfterDelay(0.25f)); // delay 0.25 detik
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDealDamage = false;
        Debug.Log("❌ WeaponDamage DISABLED (after delay)");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            attackPoint = transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, hitRange);
    }
}
