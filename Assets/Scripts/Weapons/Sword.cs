﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{
    public override void PerformAttack()
    {
        // For now, the sword does nothing extra on top of the base weapon implementation
        base.PerformAttack();
    }

    public override void PerformSpecialAbility()
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}
