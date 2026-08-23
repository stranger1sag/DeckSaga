using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDebuffEffect : Effect
{
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new RemoveDebuffGA(targets);
    }
}
