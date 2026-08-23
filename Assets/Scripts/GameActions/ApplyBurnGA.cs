using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBurnGA : GameAction
{
    public StatusEffectType Type {get; private set;} = StatusEffectType.BURN;
    public int BurnDamage {get; private set;}
    public CombatantView Target {get; private set;}
    public ApplyBurnGA(CombatantView target, int burnDamage, StatusEffectType type = StatusEffectType.BURN)
    {
        Target = target;
        BurnDamage = burnDamage;
        Type = type;
    }
}
