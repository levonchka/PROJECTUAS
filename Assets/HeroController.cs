using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    private Animator anim;
    private bool isJumping = false;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private readonly float comboMaxDelay = 1.0f;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool useRootMotion = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = useRootMotion;
    }

    void Update()
    {
        if (anim.GetBool("isDead")) return;

        HandleMovement();
        HandleJump();
        HandleAttackCombo();
        HandleDefend();
        HandleVictory();
        HandleHitInput();
        AutoResetJump();
    }

    // -------------------------------------
    // MOVEMENT
    // -------------------------------------
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
        anim.SetBool("isWalk", isMoving);

        if (isMoving && !useRootMotion)
        {
            Vector3 dir = new Vector3(h, 0, v);
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0;

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
                transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            }
        }
    }

    // -------------------------------------
    // JUMP
    // -------------------------------------
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            comboStep = 0;
            anim.SetBool("isAttack", false);
            anim.SetBool("isJump", true);
            anim.SetBool("isWalk", false);
        }
    }

    void AutoResetJump()
    {
        if (!isJumping) return;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        bool isInJump = state.IsName("JumpStart_SwordAndShield") || state.IsName("JumpSpin_SwordAndShield");

        if (isInJump && state.normalizedTime >= 0.95f)
        {
            isJumping = false;
            anim.SetBool("isJump", false);
        }
    }

    // -------------------------------------
    // ATTACK COMBO
    // -------------------------------------
    void HandleAttackCombo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!anim.GetBool("isAttack"))
            {
                anim.SetBool("isAttack", true);
                comboStep = 1;
                anim.SetInteger("attackIndex", comboStep);
                comboTimer = Time.time;
            }
            else if (comboStep < 4 && (Time.time - comboTimer) <= comboMaxDelay)
            {
                comboStep++;
                anim.SetInteger("attackIndex", comboStep);
                comboTimer = Time.time;
            }
        }

        // Reset setelah waktu habis
        if (comboStep > 0 && (Time.time - comboTimer) > comboMaxDelay)
        {
            anim.SetBool("isAttack", false);
            anim.SetInteger("attackIndex", 0);
            comboStep = 0;
        }

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Attack04_SwordAndShield") && state.normalizedTime >= 0.95f)
        {
            anim.SetBool("isAttack", false);
            anim.SetInteger("attackIndex", 0);
            comboStep = 0;
        }
    }

    // -------------------------------------
    // DEFEND
    // -------------------------------------
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

    // -------------------------------------
    // GET HIT
    // -------------------------------------
    void HandleHitInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.SetTrigger("isHit");
        }
    }

    // -------------------------------------
    // VICTORY
    // -------------------------------------
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

    // -------------------------------------
    // DIE
    // -------------------------------------
    public void Die()
    {
        anim.SetBool("isDead", true);
        anim.SetBool("isWalk", false);
        anim.SetBool("isJump", false);
        anim.SetBool("isDefend", false);
        anim.SetBool("isVictory", false);
        anim.SetBool("isAttack", false);
        anim.SetInteger("attackIndex", 0);
        comboStep = 0;
    }
}
