using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : CombatantView
{
    void OnEnable() { CardSystem.OnCardPlayed += HandleCardPlayed; }
    void OnDisable() { CardSystem.OnCardPlayed -= HandleCardPlayed; }

    // 出牌光环：每打任意牌得 X 护甲；每打攻击牌得 Y 护甲
    private void HandleCardPlayed(Card card, CombatantView caster)
    {
        if(caster != this) return;
        int onPlay = GetStatusEffectStacks(StatusEffectType.ON_PLAY_ARMOR);
        if(onPlay > 0) AddStatusEffect(StatusEffectType.ARMOR, onPlay);
        int onAttack = GetStatusEffectStacks(StatusEffectType.ON_ATTACK_PLAY_ARMOR);
        if(onAttack > 0 && card.Type == CardType.ATTACK) AddStatusEffect(StatusEffectType.ARMOR, onAttack);
    }

    public void Setup(HeroData heroData)
    {
        SetupBase(heroData.Health, heroData.Image);
    }

    // 受伤：播 Hurt 后回 Idle；死亡：播 Death
    protected override void OnDamaged()
    {
        PlayAnimation("Hurt");
        StartCoroutine(ReturnToIdle());
    }
    protected override void OnDied()
    {
        PlayAnimation("Death");
    }

    // 出牌攻击：播 Attack 后回 Idle
    public void PlayAttack()
    {
        PlayAnimation("Attack");
        StartCoroutine(ReturnToIdle());
    }

    private IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(0.45f);
        if(CurrentHealth > 0) PlayAnimation("Idle");
    }
}
