using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : CombatantView
{
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
