using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 清空目标所有燃烧层数
public class ClearBurnEffect : Effect
{
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new ClearBurnGA(targets);
    }
}
