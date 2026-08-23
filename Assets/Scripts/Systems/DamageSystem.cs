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
        // 同时结算所有目标；荆棘(目标)反击施法者；施法者吸血回血。
        foreach(var target in dealDamageGA.Targets)
        {
            int amount = dealDamageGA.Amount;
            CombatantView caster = dealDamageGA.Caster;
            if(caster != null)
            {
                amount += caster.GetStatusEffectStacks(StatusEffectType.STRENGTH);
                if(caster.GetStatusEffectStacks(StatusEffectType.WEAK) > 0)
                    amount = Mathf.RoundToInt(amount * 0.75f);
            }
            if(target.GetStatusEffectStacks(StatusEffectType.VULNERABLE) > 0)
                amount = Mathf.RoundToInt(amount * 1.5f);
            if(amount < 0) amount = 0;
            target.Damage(amount);
            // 荆棘：目标受击反弹施法者
            if(caster != null && target.GetStatusEffectStacks(StatusEffectType.THORNS) > 0)
                caster.Damage(target.GetStatusEffectStacks(StatusEffectType.THORNS));
            // 吸血：施法者按比例回血（层数为百分比）
            if(caster != null && caster.GetStatusEffectStacks(StatusEffectType.LIFESTEAL) > 0)
                caster.Heal(Mathf.RoundToInt(amount * caster.GetStatusEffectStacks(StatusEffectType.LIFESTEAL) / 100f));
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
