using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction, IHaveCaster
{
    public int Amount {get; private set;}
    public int Hits {get; private set;}
    public bool Pierce {get; private set;}
    public bool IncludeCasterBonuses {get; private set;}  // 力量/怒气等施法者加成是否由管线追加
    public List<CombatantView> Targets {get; private set;}
    public CombatantView Caster {get; private set;}
    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster, int hits = 1, bool pierce = false, bool includeCasterBonuses = true)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
        Hits = Mathf.Max(1, hits);
        Pierce = pierce;
        IncludeCasterBonuses = includeCasterBonuses;
    }
}
