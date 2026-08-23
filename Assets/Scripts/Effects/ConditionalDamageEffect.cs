using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 条件伤害：满足条件时在基础伤害上追加 bonus
// 用于 灼热核心(目标燃烧>2 时+6) / 濒死爆发(自身生命<50% 时+10) / 狂野冲撞(力量>3 时+6)
public class ConditionalDamageEffect : Effect
{
    public enum Condition { NONE, CASTER_STRENGTH_GT, TARGET_BURN_GT, CASTER_HP_PCT_LOW }
    [SerializeField] private int baseDamage;
    [SerializeField] private int bonusDamage;
    [SerializeField] private Condition condition;
    [SerializeField] private int threshold;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        int damage = baseDamage;
        if(IsConditionMet(targets, caster)) damage += bonusDamage;
        return new DealDamageGA(damage, targets, caster);
    }
    private bool IsConditionMet(List<CombatantView> targets, CombatantView caster)
    {
        CombatantView t = targets != null && targets.Count > 0 ? targets[0] : null;
        switch(condition)
        {
            case Condition.CASTER_STRENGTH_GT:
                return caster != null && caster.GetStatusEffectStacks(StatusEffectType.STRENGTH) > threshold;
            case Condition.TARGET_BURN_GT:
                return t != null && t.GetStatusEffectStacks(StatusEffectType.BURN) > threshold;
            case Condition.CASTER_HP_PCT_LOW:
                return caster != null && caster.CurrentHealth < caster.MaxHealth * threshold / 100f;
            default: return false;
        }
    }
}
