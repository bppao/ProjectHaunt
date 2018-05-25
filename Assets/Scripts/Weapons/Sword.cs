using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{
    public override void PerformAttack()
    {
        // Don't allow multiple attacks to queue up if the player is spamming the attack button
        if (IsAttacking) return;

        Animator.SetTrigger(BASE_ATTACK_ANIM_TRIGGER);
    }

    public override void PerformSpecialAbility()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}
