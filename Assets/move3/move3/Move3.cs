using UnityEngine;

public class Move3 : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Animator anim;
    bool isHit = false;
    float hitTimer = 0f;
    float hitDuration = 0.7f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHit)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitDuration)
            {
                isHit = false;
                hitTimer = 0;
                anim.SetBool("jalan", false);
            }
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
        anim.SetBool("jalan", isMoving);

        if (isMoving)
        {
            Vector3 targetDirection = new Vector3(h, 0, v);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            transform.position += targetDirection.normalized * speed * Time.deltaTime;
        }

        // lompat
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("lompat", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.SetBool("lompat", false);
        }
    }

    // animasi kena pukulan dari skeleton
    public void TakeHit()
    {
        if (isHit) return;
        isHit = true;
        hitTimer = 0f;
        anim.Play("hit");
    }
}
