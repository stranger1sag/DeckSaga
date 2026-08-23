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
        // 攻击动画在出牌流程(PlayCardPerformer)触发；此处不再播 caster 攻击动画
        // (避免反击/道具等以英雄为 caster 的伤害误播攻击动画)
        // 同时结算：所有目标同一帧掉血+播放受伤动画（不再逐个 yield 等待）
        foreach(var target in dealDamageGA.Targets)
            target.Damage(dealDamageGA.Amount);
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
