using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTM : TargetMode
{
    public override List<CombatantView> GetTargets()
    {
        return new List<CombatantView> { HeroSystem.Instance.HeroView };
    }
}