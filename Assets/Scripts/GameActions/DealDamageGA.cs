using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction, IHaveCaster
{
    public int Amount {get; private set;}
    public int Hits {get; private set;}
    public List<CombatantView> Targets {get; private set;}
    public CombatantView Caster {get; private set;}
    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster, int hits = 1)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
        Hits = Mathf.Max(1, hits);
    }
}
