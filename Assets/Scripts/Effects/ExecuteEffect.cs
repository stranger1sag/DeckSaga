using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 处决：目标当前生命 <= 阈值 直接斩杀，否则造成基础伤害
// 用于 处决(生命<20斩杀/否则8伤) / 刀刃(生命<30斩杀) / 火刑(燃烧叠3时斩杀)
public class ExecuteEffect : Effect
{
    [SerializeField] private int baseDamage;
    [SerializeField] private int executeThreshold;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new ExecuteGA(baseDamage, executeThreshold, targets, caster);
    }
}
