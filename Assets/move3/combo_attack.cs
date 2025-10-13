using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    private float comboAtk = 0f;
    private float lastAtk = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            lastAtk = Time.time;
            comboAtk++;
            comboAtk = Mathf.Clamp(comboAtk, 0, 3);
            anim.SetFloat("attack", comboAtk);
        }

        // reset serangan
        if (Time.time - lastAtk >= 1f && comboAtk != 0)
        {
            comboAtk = 0;
            anim.SetFloat("attack", comboAtk);
        }
    }
}
