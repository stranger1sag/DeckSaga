using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk
{
    public Sprite Image => PerkData.Image;
    private readonly PerkCondition PerkCondition;
    private readonly AutoTargetEffect AutoTargetEffect;
    private readonly PerkData PerkData;
    public Perk(PerkData perkData)
    {
        PerkData = perkData;
        PerkCondition = perkData.PerkCondition;
        AutoTargetEffect = perkData.AutoTargetEffect;
    }
    public void OnAdd()
    {
        PerkCondition.SubscribeCondition(Reaction);
    }
    public void OnRemove()
    {
        PerkCondition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        if (PerkCondition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();
            if(PerkData.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }
            if(PerkData.UseAutoTarget)
            {
                targets.AddRange(AutoTargetEffect.TargetMode.GetTargets());
            }
            GameAction perkEffectAction = AutoTargetEffect.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}
