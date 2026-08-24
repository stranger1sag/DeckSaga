using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 从手牌/牌库移除一张牌（删牌机制）
// 用于 剔伐(删除一张手牌，回6血) / 精血(删除一张牌，抽2)
public class RemoveFromDeckEffect : Effect
{
    [SerializeField] private int count = 1;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new RemoveFromDeckGA(count);
    }
}
