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
        // 施法者(英雄)出牌攻击时播放攻击动画
        if(dealDamageGA.Caster != null) dealDamageGA.Caster.PlayAttack();
        foreach(var target in dealDamageGA.Targets)
        {
            target.Damage(dealDamageGA.Amount);
            //Instantiate(DamageVFX, target.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.15f);
            if(target.CurrentHealth <= 0)
            {
                if(target is EnemyView enemyView)
                {
                    KillEnemyGA killEnemyGA = new(enemyView);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
            }
        }
    }
}
