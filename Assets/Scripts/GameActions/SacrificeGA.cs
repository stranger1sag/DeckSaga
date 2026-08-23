using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeGA : GameAction
{
    public int Amount {get; private set;}
    public List<CombatantView> Targets {get; private set;}
    public SacrificeGA(int amount, List<CombatantView> targets)
    {
        Amount = amount;
        Targets = new(targets);
    }
}
