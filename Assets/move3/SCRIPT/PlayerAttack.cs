using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.4f;
    public float attackRange = 2.8f;       // diperbesar jangkauannya
    public float attackDamage = 25f;
    public LayerMask enemyLayer;

    private Animator anim;
    private float lastAttackTime = -999f;
    private int comboStep = 0;
    private float comboResetTimer = 0f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryAttack();

        if (Time.time - comboResetTimer > 1.2f)
        {
            comboStep = 0;
            anim.SetInteger("comboStep", 0);
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;
        comboResetTimer = Time.time;

        comboStep++;
        if (comboStep > 2) comboStep = 0;
        anim.SetInteger("comboStep", comboStep);

        anim.ResetTrigger("doAttack");
        anim.SetTrigger("doAttack");

        Invoke(nameof(DealDamageArea), 0.25f);
    }

    void DealDamageArea()
    {
        Vector3 attackPos = transform.position + transform.forward * 1.3f;
        Collider[] hits = Physics.OverlapSphere(attackPos, attackRange, enemyLayer);

        if (hits.Length == 0)
        {
            Debug.Log("⚠️ Tidak ada musuh dalam jangkauan serangan!");
            return;
        }

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"💥 {enemy.name} terkena {attackDamage} damage!");
            }
            else
            {
                Debug.Log($"🚫 {hit.name} tidak punya EnemyHealth component!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPos = transform.position + transform.forward * 1.3f;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
