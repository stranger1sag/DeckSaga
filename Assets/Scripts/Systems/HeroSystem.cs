using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView {get; private set;}
    void OnEnable()
    {
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
    }
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        // 回合末格挡清零（受护体 WARD 保护时不清）
        int armor = HeroView.GetStatusEffectStacks(StatusEffectType.ARMOR);
        if(armor > 0 && HeroView.GetStatusEffectStacks(StatusEffectType.WARD) <= 0)
            HeroView.RemoveStatusEffect(StatusEffectType.ARMOR, armor);
        // 回合末清掉「本回合」的怒气 + 出手光环 + 敏捷
        int fury = HeroView.GetStatusEffectStacks(StatusEffectType.FURY);
        if(fury > 0) HeroView.RemoveStatusEffect(StatusEffectType.FURY, fury);
        int dex = HeroView.GetStatusEffectStacks(StatusEffectType.DEXTERITY);
        if(dex > 0) HeroView.RemoveStatusEffect(StatusEffectType.DEXTERITY, dex);
        foreach (var turnBuff in new StatusEffectType[]{StatusEffectType.ON_PLAY_ARMOR, StatusEffectType.ON_ATTACK_PLAY_ARMOR}) {
            int s = HeroView.GetStatusEffectStacks(turnBuff);
            if(s > 0) HeroView.RemoveStatusEffect(turnBuff, s);
        }
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if(burnStacks > 0)
        {
            ApplyBurnGA applyBurnGA = new(HeroView, burnStacks);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }
        // 回合开始：Power 本场光环触发
        int perTurnArmor = HeroView.GetStatusEffectStacks(StatusEffectType.PER_TURN_ARMOR);
        if(perTurnArmor > 0) HeroView.AddStatusEffect(StatusEffectType.ARMOR, perTurnArmor);
        int perTurnStrength = HeroView.GetStatusEffectStacks(StatusEffectType.PER_TURN_STRENGTH);
        if(perTurnStrength > 0) HeroView.AddStatusEffect(StatusEffectType.STRENGTH, perTurnStrength);
        int perTurnBurn = HeroView.GetStatusEffectStacks(StatusEffectType.PER_TURN_BURN);
        if(perTurnBurn > 0) AddBurnToAllEnemies(perTurnBurn);
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }

    private void AddBurnToAllEnemies(int amount)
    {
        foreach(var enemy in FindObjectsOfType<EnemyView>())
            enemy.AddStatusEffect(StatusEffectType.BURN, amount);
    }
}
