using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 处决：目标当前生命 <= 阈值 直接斩杀，否则造成基础伤害
public class ExecuteGA : GameAction, IHaveCaster
{
    public int BaseDamage {get; private set;}
    public int Threshold {get; private set;}
    public List<CombatantView> Targets {get; private set;}
    public CombatantView Caster {get; private set;}
    public ExecuteGA(int baseDamage, int threshold, List<CombatantView> targets, CombatantView caster)
    {
        BaseDamage = baseDamage;
        Threshold = threshold;
        Targets = targets;
        Caster = caster;
    }
}
