using UnityEngine;

public class AttackEventHandler : MonoBehaviour
{
    public WeaponDamage weaponDamage; // <- HARUS public dan bukan GameObject
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void EnableDamage()
    {
        if (weaponDamage != null)
            weaponDamage.EnableDamage();
    }

    public void DisableDamage()
    {
        if (weaponDamage != null)
            weaponDamage.DisableDamage();
    }
}
