using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Sword m_Sword;

    private Transform m_PlayerHand;
    private BaseWeapon m_EquippedWeapon;

    private void Start()
    {
        // Assumes the player hand transform is the first child
        m_PlayerHand = transform.GetChild(0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("EquipWeapon"))
        {
            EquipWeapon();
        }

        if (Input.GetButtonDown("BaseAttack"))
        {
            if (m_EquippedWeapon == null) return;
            m_EquippedWeapon.PerformAttack();
        }
    }

    private void EquipWeapon()
    {
        if (m_EquippedWeapon != null)
        {
            Debug.Log("Weapon already equipped!");
            return;
        }

        Debug.Log("Weapon equipped!");
        m_EquippedWeapon = Instantiate(
            m_Sword,
            m_PlayerHand.position,
            m_PlayerHand.rotation,
            m_PlayerHand);
    }
}
