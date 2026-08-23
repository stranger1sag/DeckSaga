using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSystem : MonoBehaviour
{
    [SerializeField] private GameObject healVFX;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<HealGA>(HealPerformer);
        ActionSystem.AttachPerformer<SacrificeGA>(SacrificePerformer);
        ActionSystem.AttachPerformer<RemoveDebuffGA>(RemoveDebuffPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<HealGA>();
        ActionSystem.DetachPerformer<SacrificeGA>();
        ActionSystem.DetachPerformer<RemoveDebuffGA>();
    }
    private IEnumerator HealPerformer(HealGA healGA)
    {
        foreach(var target in healGA.Targets)
            target.Heal(healGA.Amount);
        yield return null;
    }
    private IEnumerator SacrificePerformer(SacrificeGA sacrificeGA)
    {
        foreach(var target in sacrificeGA.Targets)
            target.Damage(sacrificeGA.Amount);
        yield return null;
    }
    private IEnumerator RemoveDebuffPerformer(RemoveDebuffGA removeDebuffGA)
    {
        foreach(var target in removeDebuffGA.Targets)
        {
            // 移除负面状态
            foreach (var debuff in new StatusEffectType[]{
                StatusEffectType.WEAK, StatusEffectType.VULNERABLE, StatusEffectType.BURN,
                StatusEffectType.POISON, StatusEffectType.STUN, StatusEffectType.TAUNT,
                StatusEffectType.ARMOR_BREAK, StatusEffectType.DEPLETE })
            {
                int stacks = target.GetStatusEffectStacks(debuff);
                if(stacks > 0) target.RemoveStatusEffect(debuff, stacks);
            }
        }
        yield return null;
    }
}
