using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;
    public int AttackPower {get; set;}
    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        UpdateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);
    }
    private void UpdateAttackText()
    {
        attackText.text = "ATK: " + AttackPower.ToString();
    }
    // 怪物受伤：闪白一下（SpriteRenderer 颜色提到白再回原色）。英雄有 Hurt 动画不走这里
    protected override void OnDamaged()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if(sr != null)
            sr.DOColor(Color.white, 0.08f).SetLoops(2, LoopType.Yoyo);
    }
}
