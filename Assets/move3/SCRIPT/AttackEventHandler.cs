using UnityEngine;

public class AttackEventHandler : MonoBehaviour
{
    public WeaponDamage weaponDamage; // assign dari inspector (pedang)
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // 🎯 Dipanggil dari event animasi
    public void EnableDamage()
    {
        if (weaponDamage != null)
        {
            weaponDamage.EnableDamage();
            Debug.Log("🗡️ Damage aktif via AttackEventHandler");
        }
    }

    // 🎯 Dipanggil dari event animasi
    public void DisableDamage()
    {
        if (weaponDamage != null)
        {
            weaponDamage.DisableDamage();
            Debug.Log("❌ Damage nonaktif via AttackEventHandler");
        }
    }
}
