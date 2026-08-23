using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 按施法者当前护甲算伤害：damage = base + 护甲×armorMult
// 用于 盾击(6+护甲) / 方阵(护甲×0.5全体) / 荣耀奋战(护甲×1全体)
public class ArmorDamageEffect : Effect
{
    [SerializeField] private int baseDamage;
    [SerializeField] private float armorMult = 1f;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        int armor = caster != null ? caster.GetStatusEffectStacks(StatusEffectType.ARMOR) : 0;
        int damage = baseDamage + Mathf.RoundToInt(armor * armorMult);
        return new DealDamageGA(damage, targets, caster);
    }
}
