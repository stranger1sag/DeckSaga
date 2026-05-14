using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBurnGA : GameAction
{
    public int BurnDamage {get; private set;}
    public CombatantView Target {get; private set;}
    public ApplyBurnGA(CombatantView target, int burnDamage)
    {
        Target = target;
        BurnDamage = burnDamage;
    }
}
