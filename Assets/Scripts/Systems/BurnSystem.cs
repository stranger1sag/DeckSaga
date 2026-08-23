using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject burnVFX;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
        ActionSystem.AttachPerformer<ClearBurnGA>(ClearBurnPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGA>();
        ActionSystem.DetachPerformer<ClearBurnGA>();
    }
    private IEnumerator ClearBurnPerformer(ClearBurnGA clearBurnGA)
    {
        foreach(var target in clearBurnGA.Targets)
        {
            int burn = target.GetStatusEffectStacks(StatusEffectType.BURN);
            if(burn > 0) target.RemoveStatusEffect(StatusEffectType.BURN, burn);
        }
        yield return null;
    }
    private IEnumerator ApplyBurnPerformer(ApplyBurnGA applyBurnGA)
    {
        CombatantView combatantView = applyBurnGA.Target;
        combatantView.Damage(applyBurnGA.BurnDamage);
        // BURN 回合末减层；POISON 不减(持续掉)
        if(applyBurnGA.Type == StatusEffectType.BURN)
            combatantView.RemoveStatusEffect(StatusEffectType.BURN,1);
        yield return new WaitForSeconds(1f);
    }
}
