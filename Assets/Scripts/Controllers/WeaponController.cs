using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private BaseWeapon m_WeaponToEquip;

    private Transform m_PlayerHand;
    private BaseWeapon m_EquippedWeapon;
    public BaseWeapon EquippedWeapon { get { return m_EquippedWeapon; } }
    public bool IsWeaponEquipped { get { return m_EquippedWeapon != null; } }

    private void Start()
    {
        // Find the player hand transform
        m_PlayerHand = GameObject.Find(name + "Hand").transform;
    }

    private void Update()
    {
        if (Input.GetButtonDown("EquipWeapon"))
        {
            EquipWeapon();
        }

        if (Input.GetButtonDown("BaseAttack"))
        {
            if (!IsWeaponEquipped) return;
            m_EquippedWeapon.PerformAttack();
        }
    }

    private void EquipWeapon()
    {
        if (IsWeaponEquipped)
        {
            Debug.Log("Weapon already equipped!");
            return;
        }

        Debug.Log("Weapon equipped!");
        m_EquippedWeapon = Instantiate(
            m_WeaponToEquip,
            m_PlayerHand.position,
            m_PlayerHand.rotation,
            m_PlayerHand);
    }
}
