using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;
    [SerializeField] private int hits = 1;   // 多段：一次打出 hits 次
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new DealDamageGA(damageAmount, targets, caster, hits);
    }
}
