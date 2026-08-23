using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 卡牌类型：用于按类型生效的机制（如「本回合攻击牌+X」、POWER 常驻光环）
public enum CardType
{
    ATTACK,     // 攻击牌：造成伤害为主
    SKILL,      // 技能牌：格挡/增益/抽牌/牌库操作，非伤害
    POWER,      // 能力牌：打出后本场持续生效的光环
    STATUS,     // 状态牌：临时负面牌
    CURSE,      // 诅咒牌：塞进卡组的负面牌
}
