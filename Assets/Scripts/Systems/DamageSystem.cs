using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private GameObject DamageVFX;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>(  );
    }
    private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        // 伤害管线：基础伤害 -> (施法者力量/虚弱) -> (目标易伤) -> 结算
        // 多段(hits)重复结算；同时结算所有目标；荆棘(目标)反击施法者；施法者吸血回血。
        int reps = Mathf.Max(1, dealDamageGA.Hits);
        for(int r = 0; r < reps; r++)
        {
            foreach(var target in dealDamageGA.Targets)
            {
                int amount = dealDamageGA.Amount;
                CombatantView caster = dealDamageGA.Caster;
                // 施法者加成（力量/怒气/下张攻击/虚弱）仅在未预烘焙时由管线追加（力量类/护甲类效果已把缩放算进Amount）
                if(caster != null && dealDamageGA.IncludeCasterBonuses)
                {
                    amount += caster.GetStatusEffectStacks(StatusEffectType.STRENGTH)
                            + caster.GetStatusEffectStacks(StatusEffectType.FURY);
                    // 下张攻击加成：仅首段命中加值并消耗
                    if(r == 0 && caster.GetStatusEffectStacks(StatusEffectType.NEXT_ATTACK_BONUS) > 0)
                    {
                        amount += caster.GetStatusEffectStacks(StatusEffectType.NEXT_ATTACK_BONUS);
                        caster.RemoveStatusEffect(StatusEffectType.NEXT_ATTACK_BONUS,
                            caster.GetStatusEffectStacks(StatusEffectType.NEXT_ATTACK_BONUS));
                    }
                    if(caster.GetStatusEffectStacks(StatusEffectType.WEAK) > 0)
                        amount = Mathf.RoundToInt(amount * 0.75f);
                }
                if(target.GetStatusEffectStacks(StatusEffectType.VULNERABLE) > 0)
                    amount = Mathf.RoundToInt(amount * 1.5f);
                if(amount < 0) amount = 0;
                target.Damage(amount, dealDamageGA.Pierce);
                // 荆棘：目标受击反弹施法者
                if(caster != null && target.GetStatusEffectStacks(StatusEffectType.THORNS) > 0)
                    caster.Damage(target.GetStatusEffectStacks(StatusEffectType.THORNS));
                // 吸血：施法者按比例回血（层数+血之契约光环为百分比）
                int lifestealPct = (caster != null ? caster.GetStatusEffectStacks(StatusEffectType.LIFESTEAL) : 0)
                                 + (caster != null ? caster.GetStatusEffectStacks(StatusEffectType.EXTRA_LIFESTEAL) : 0);
                if(caster != null && lifestealPct > 0)
                    caster.Heal(Mathf.RoundToInt(amount * lifestealPct / 100f));
            }
            if(r < reps - 1) yield return new WaitForSeconds(0.1f);
        }
        // 让受伤表现播一小会，再统一结算死亡
        yield return new WaitForSeconds(0.2f);
        foreach(var target in dealDamageGA.Targets)
        {
            if(target.CurrentHealth <= 0 && target is EnemyView enemyView)
            {
                KillEnemyGA killEnemyGA = new(enemyView);
                ActionSystem.Instance.AddReaction(killEnemyGA);
            }
        }
    }
}
