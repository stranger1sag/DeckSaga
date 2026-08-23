using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealGA : GameAction
{
    public int Amount {get; private set;}
    public List<CombatantView> Targets {get; private set;}
    public HealGA(int amount, List<CombatantView> targets)
    {
        Amount = amount;
        Targets = new(targets);
    }
}
