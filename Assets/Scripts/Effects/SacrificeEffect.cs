using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeEffect : Effect
{
    [SerializeField] private int sacrificeAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new SacrificeGA(sacrificeAmount, targets);
    }
}
