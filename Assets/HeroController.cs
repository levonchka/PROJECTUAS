using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = 20f;

    private Vector3 moveDir = Vector3.zero;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
    private bool isAttacking = false;

    private int comboStep = 0;
    private float comboTimer = 0f;
    private readonly float comboMaxDelay = 1.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.applyRootMotion = false;
    }

    void Update()
    {
        if (anim.GetBool("isDead")) return;

        HandleMovement();
        HandleJump();
        HandleAttackCombo();
        HandleDefend();
        HandleHit();
        HandleVictory();
    }

    // =============================
    // MOVEMENT
    // =============================
    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        // masih bisa jalan walau attack tapi pelan
        float currentSpeed = isAttacking ? moveSpeed * 0.6f : moveSpeed;

        if (input.magnitude >= 0.1f)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = (camForward * v + camRight * h).normalized;
            moveDir = move;

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            anim.SetBool("isWalk", true);
        }
        else
        {
            moveDir = Vector3.zero;
            anim.SetBool("isWalk", false);
        }

        Vector3 velocity = moveDir * currentSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    // =============================
    // JUMP
    // =============================
    void HandleJump()
    {
        bool grounded = controller.isGrounded;

        if (grounded)
        {
            if (isJumping)
            {
                isJumping = false;
                if (!isAttacking)
                    anim.CrossFade("Idle_Normal_SwordAndShield", 0.1f);
            }

            verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                isJumping = true;
                anim.CrossFade("JumpFull_Spin_RM_SwordAndShield", 0.05f);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    // =============================
    // ATTACK (Attack01, Attack04, dan Aerial Attack)
    // =============================
    void HandleAttackCombo()
    {
        // izinkan attack di darat atau di udara
        if (Input.GetMouseButtonDown(0))
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

            // --- Attack di udara ---
            if (isJumping && !isAttacking)
            {
                isAttacking = true;
                anim.SetBool("isAttack", true);
                anim.CrossFade("Attack_Air_SwordAndShield", 0.05f);
                comboTimer = Time.time;
                return;
            }

            // --- Attack di darat (combo) ---
            if (!isAttacking)
            {
                isAttacking = true;
                comboStep = 1;
                anim.SetBool("isAttack", true);
                anim.SetInteger("attackIndex", comboStep);
                anim.CrossFade("Attack01_SwordAndShield", 0.05f);
                comboTimer = Time.time;
            }
            else if (isAttacking && comboStep == 1 && state.normalizedTime >= 0.3f)
            {
                comboStep = 4;
                anim.SetInteger("attackIndex", comboStep);
                anim.CrossFade("Attack04_SwordAndShield", 0.05f);
                comboTimer = Time.time;
            }
        }

        // Reset combo kalau timeout
        if (isAttacking && (Time.time - comboTimer) > comboMaxDelay)
        {
            ResetAttack();
        }

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if ((info.IsName("Attack01_SwordAndShield") || info.IsName("Attack04_SwordAndShield") || info.IsName("Attack_Air_SwordAndShield"))
            && info.normalizedTime >= 0.95f)
        {
            ResetAttack();
        }
    }

    void ResetAttack()
    {
        anim.SetBool("isAttack", false);
        anim.SetInteger("attackIndex", 0);
        comboStep = 0;
        isAttacking = false;

        if (!isJumping)
            anim.CrossFade("Idle_Normal_SwordAndShield", 0.1f);
    }

    // =============================
    // DEFEND
    // =============================
    void HandleDefend()
    {
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("isDefend", true);
            anim.SetBool("isWalk", false);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("isDefend", false);
        }
    }

    // =============================
    // HIT
    // =============================
    void HandleHit()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("isHit");
        }
    }

    // =============================
    // VICTORY
    // =============================
    void HandleVictory()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetBool("isVictory", true);
            anim.SetBool("isWalk", false);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            anim.SetBool("isVictory", false);
        }
    }

    // =============================
    // DIE
    // =============================
    public void Die()
    {
        anim.SetBool("isDead", true);
        anim.SetBool("isWalk", false);
        anim.SetBool("isDefend", false);
        anim.SetBool("isVictory", false);
        anim.SetBool("isAttack", false);
        anim.SetInteger("attackIndex", 0);
        comboStep = 0;
        isAttacking = false;
        anim.CrossFade("Die01_SwordAndShield", 0.1f);
    }
}
