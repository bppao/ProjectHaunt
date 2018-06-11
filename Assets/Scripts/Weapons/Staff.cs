using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : BaseRangedWeapon
{
    public override void PerformAttack()
    {
        // For now, the staff does nothing extra on top of the base weapon implementation
        base.PerformAttack();
    }

    public override void PerformSpecialAbility()
    {
        throw new NotImplementedException();
    }
}
