using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;

    // attack state tracker (used to slow movement while attacking)
    private bool isAttacking = false;

    private Vector3 moveDir = Vector3.zero;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.applyRootMotion = false;

        currentHealth = maxHealth;
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        float currentSpeed = isAttacking ? moveSpeed * 0.6f : moveSpeed;

        if (input.magnitude >= 0.1f)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0; camRight.y = 0;
            camForward.Normalize(); camRight.Normalize();

            moveDir = (camForward * v + camRight * h).normalized;

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

    void HandleJump()
    {
        bool grounded = controller.isGrounded;

        if (grounded)
        {
            if (isJumping)
            {
                isJumping = false;
                // let animator fall back to idle/walk through transitions
            }

            verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                isJumping = true;
                anim.CrossFade("JumpFull_Spin_RM_SwordAndShield", 0.05f);
            }
            else
            {
                anim.SetBool("isJump", false);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            anim.SetBool("isJump", true);
        }
    }

    // PUBLIC: dipanggil PlayerAttack saat attack valid (cooldown ok)
    // clipName harus sama persis dengan nama clip/state di Animator layer 1
    public void PlayUpperAttack(string clipName)
    {
        if (isDead) return;

        // play on layer 1 (attack upper)
        anim.CrossFadeInFixedTime(clipName, 0.06f, 1);

        // set flag agar movement sedikit dikurangi saat attack
        StopAllCoroutines();
        float len = GetAnimationClipLength(clipName);
        if (len <= 0f) len = 0.6f; // fallback
        isAttacking = true;
        StartCoroutine(ResetIsAttackingAfter(len * 0.9f));
    }

    IEnumerator ResetIsAttackingAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    float GetAnimationClipLength(string clipName)
    {
        if (anim.runtimeAnimatorController == null) return 0f;
        var clips = anim.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
            if (clips[i].name == clipName)
                return clips[i].length;
        return 0f;
    }

    // health methods remain same
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar) healthBar.value = currentHealth;
        anim.SetTrigger("isHit");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        anim.CrossFade("Die01_SwordAndShield", 0.1f);
    }
}
