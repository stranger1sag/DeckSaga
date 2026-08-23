using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject burnVFX;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGA>();
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
