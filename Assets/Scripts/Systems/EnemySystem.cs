using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView;
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }
    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (EnemyData enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }
    //Performers
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        foreach(EnemyView enemyView in enemyBoardView.EnemyViews)
        {
            int BurnStacks = enemyView.GetStatusEffectStacks(StatusEffectType.BURN);
            if(BurnStacks>0)
            {
                ApplyBurnGA applyBurnGA = new(enemyView, BurnStacks);
                ActionSystem.Instance.AddReaction(applyBurnGA);
            }
            int PoisonStacks = enemyView.GetStatusEffectStacks(StatusEffectType.POISON);
            if(PoisonStacks>0)
            {
                ApplyBurnGA poisonGA = new(enemyView, PoisonStacks, StatusEffectType.POISON);
                ActionSystem.Instance.AddReaction(poisonGA);
            }
            // 眩晕：跳过本回合行动并减1层
            if(enemyView.GetStatusEffectStacks(StatusEffectType.STUN) > 0)
            {
                enemyView.RemoveStatusEffect(StatusEffectType.STUN, 1);
                continue;
            }
            AttackHeroGA attackGA = new(enemyView);
            ActionSystem.Instance.AddReaction(attackGA);
        }
        yield return null;
    }
    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        EnemyView attacker = attackHeroGA.Attacker;
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.15f);
        // 枯竭：攻击力 - 层数（最少0）
        int atkPower = Mathf.Max(0, attacker.AttackPower - attacker.GetStatusEffectStacks(StatusEffectType.DEPLETE));
        // 嘲讽：有 TAUNT 的目标优先被攻击，否则攻击英雄
        CombatantView target = HeroSystem.Instance.HeroView;
        foreach (var cv in UnityEngine.Object.FindObjectsOfType<CombatantView>())
            if (cv.GetStatusEffectStacks(StatusEffectType.TAUNT) > 0) { target = cv; break; }
        DealDamageGA damageGA = new(atkPower,new(){target},attackHeroGA.Caster);
        ActionSystem.Instance.AddReaction(damageGA);
    }
    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);
    }
}
