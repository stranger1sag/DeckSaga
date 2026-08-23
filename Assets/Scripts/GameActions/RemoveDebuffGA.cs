using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDebuffGA : GameAction
{
    public List<CombatantView> Targets {get; private set;}
    public RemoveDebuffGA(List<CombatantView> targets)
    {
        Targets = new(targets);
    }
}
