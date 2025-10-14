using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int currentPoint = 0;

    [Header("Combat Settings")]
    public float speed = 2f;
    public float chaseRange = 6f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    private float lastAttackTime;

    [Header("Model Orientation Fix")]
    [Tooltip("If model faces +X instead of +Z, use -90. If faces -X, use 90. If faces -Z, use 180.")]
    public float facingOffsetY = -90f;

    private Transform player;
    private Animator anim;
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponentInChildren<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (anim != null)
            anim.applyRootMotion = false;
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.currentHealth <= 0) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Rotasi selalu hadap ke player kalau dalam jarak chase/attack
        if (distance <= chaseRange)
            FaceTarget(player.position);

        if (distance <= attackRange)
            Attack();
        else if (distance <= chaseRange)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        if (anim != null) anim.SetBool("isWalk", true);

        Transform target = patrolPoints[currentPoint];
        MoveTowardsTarget(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void ChasePlayer()
    {
        if (anim != null) anim.SetBool("isWalk", true);
        MoveTowardsTarget(player.position);
    }

    void Attack()
    {
        if (anim != null) anim.SetBool("isWalk", false);

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (anim != null) anim.SetTrigger("isAttack");
            lastAttackTime = Time.time;
            Invoke(nameof(DealDamage), 0.45f); // Sesuaikan timing dengan animasi pukul
        }
    }

    void DealDamage()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= attackRange + 0.3f)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(10);

                // 🔥 Tambahkan animasi kena hit ke hero
                Animator heroAnim = player.GetComponentInChildren<Animator>();
                if (heroAnim != null)
                    heroAnim.SetTrigger("isHit"); // Pastikan parameternya ada di Animator hero
            }
        }
    }

    void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        FaceTarget(target);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(0, facingOffsetY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
