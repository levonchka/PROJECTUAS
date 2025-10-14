using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float attackRange = 2f;
    public float chaseRange = 8f;
    public float attackCooldown = 2f;
    public Transform[] patrolPoints;

    private Transform hero;
    private Animator animator;
    private int currentPoint = 0;
    private float lastAttackTime;
    private bool isAttacking = false;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    void Start()
    {
        animator = GetComponent<Animator>();
        hero = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length > 0)
            transform.position = patrolPoints[0].position;
    }

    void Update()
    {
        if (hero == null) return;

        float distance = Vector3.Distance(transform.position, hero.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol(distance);
                break;
            case State.Chase:
                Chase(distance);
                break;
            case State.Attack:
                Attack(distance);
                break;
        }
    }

    void Patrol(float distance)
    {
        animator.SetBool("isWalk", true);
        animator.SetBool("isAttack", false);

        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPoint];
        MoveTowards(targetPoint.position, patrolSpeed);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;

        if (distance <= chaseRange)
            ChangeState(State.Chase);
    }

    void Chase(float distance)
    {
        animator.SetBool("isWalk", true);
        animator.SetBool("isAttack", false);

        MoveTowards(hero.position, chaseSpeed);

        if (distance <= attackRange)
            ChangeState(State.Attack);
        else if (distance > chaseRange * 1.2f)
            ChangeState(State.Patrol);
    }

    void Attack(float distance)
    {
        Vector3 dir = hero.position - transform.position;
        dir.y = 0;
        if (dir.magnitude > 0.1f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 8f);
        }

        animator.SetBool("isWalk", false);
        animator.SetBool("isAttack", true);

        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            isAttacking = true;
        }

        if (distance > attackRange + 1.5f)
        {
            ChangeState(State.Chase);
            isAttacking = false;
        }
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir.magnitude > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            transform.position += dir.normalized * speed * Time.deltaTime;
        }
    }

    void ChangeState(State newState) => currentState = newState;

    // Event animasi attack
    public void DealDamage()
    {
        if (hero == null) return;

        float distance = Vector3.Distance(transform.position, hero.position);
        if (distance <= attackRange + 0.5f)
        {
            Move3 heroScript = hero.GetComponent<Move3>();
            if (heroScript != null)
            {
                heroScript.TakeHit(); // Hero kena animasi hit
            }
        }
        isAttacking = false;
    }
}
