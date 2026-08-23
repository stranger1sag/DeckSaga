using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainManaEffect : Effect
{
    [SerializeField] private int manaAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new GainManaGA(manaAmount);
    }
}
