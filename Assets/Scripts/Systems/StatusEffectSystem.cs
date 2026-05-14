using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectSystem  : Singleton<StatusEffectSystem>
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }
    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        foreach(var target in addStatusEffectGA.Targets)
        {
            target.AddStatusEffect(addStatusEffectGA.StatusEffectType,addStatusEffectGA.StackCount);
        }
        yield return null;
    }
}
