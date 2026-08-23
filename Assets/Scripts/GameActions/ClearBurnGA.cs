using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 清空目标所有燃烧层数
public class ClearBurnGA : GameAction
{
    public List<CombatantView> Targets {get; private set;}
    public ClearBurnGA(List<CombatantView> targets)
    {
        Targets = targets;
    }
}
