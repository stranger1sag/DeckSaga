using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 燃烧缩放伤害：damage = base + burnMult × 全场燃烧总层数
// 用于 无尽火雨(2×全场燃烧)
public class BurnScaledDamageEffect : Effect
{
    [SerializeField] private int baseDamage;
    [SerializeField] private float burnMult = 1f;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        int totalBurn = 0;
        if(targets != null)
            foreach(var t in targets)
                totalBurn += t.GetStatusEffectStacks(StatusEffectType.BURN);
        int damage = baseDamage + Mathf.RoundToInt(totalBurn * burnMult);
        return new DealDamageGA(damage, targets, caster);
    }
}
