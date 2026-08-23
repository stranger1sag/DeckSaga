using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 按施法者力量算伤害：damage = base + 力量×strengthMult
// 用于 万兽狂潮(2×(力量+3)=base6+2×力量) / 嗜血战刃(每点力量=2伤打全体=base0+2×力量)
public class StrengthDamageEffect : Effect
{
    [SerializeField] private int baseDamage;
    [SerializeField] private float strengthMult = 1f;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        int strength = caster != null ? caster.GetStatusEffectStacks(StatusEffectType.STRENGTH) : 0;
        int damage = baseDamage + Mathf.RoundToInt(strength * strengthMult);
        return new DealDamageGA(damage, targets, caster, 1, false, false);
    }
}
