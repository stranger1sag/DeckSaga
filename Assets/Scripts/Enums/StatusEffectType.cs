using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 状态效果类型。数字管线：伤害/格挡计算会读取这些。
public enum StatusEffectType
{
    // 已有
    ARMOR,          // 格挡：抵伤，回合结束清零
    BURN,           // 燃烧：回合末 X 伤，层数-1
    // 玩家增益(buffs)
    STRENGTH,       // 力量：攻击伤害 + 层数
    DEXTERITY,      // 敏捷：获得格挡 + 层数
    THORNS,         // 荆棘：受击时反击攻击者 层数
    LIFESTEAL,      // 吸血：造成伤害按 %回血
    FURY,           // 怒气：本回合每张攻击牌 +X
    WARD,           // 护体：格挡不随回合结束清零(持续格挡)
    // 敌人减益(debuffs)
    VULNERABLE,     // 易伤：受到伤害 +50%
    WEAK,           // 虚弱：攻击伤害 -25%
    POISON,         // 中毒：回合末 X 伤，层数不减(持续掉)
    STUN,           // 眩晕：跳过下回合行动
    TAUNT,          // 嘲讽：强制敌人攻击该目标
    ARMOR_BREAK,    // 破甲：无视格挡/削格挡
    DEPLETE,        // 枯竭：攻击力 -1/层(叠加)
    ON_PLAY_ARMOR,        // 出牌光环：每打出任意一张牌获得X护甲
    ON_ATTACK_PLAY_ARMOR, // 出牌光环：每打出一张攻击牌获得X护甲
    // 能力牌(POWER)本场光环：每回合开始触发
    PER_TURN_ARMOR,       // 本场：每回合开始 得X格挡（持久战意）
    PER_TURN_STRENGTH,    // 本场：每回合开始 得X力量（狂血沸腾）
    EXTRA_LIFESTEAL,      // 本场：每次命中敌人 额外+X吸血（血之契约）
    PER_TURN_BURN,        // 本场：每回合开始 给所有敌 +X燃烧（灼热流转）
    NEXT_ATTACK_BONUS,    // 下张攻击牌 伤害+X（嗜血渴望）
}
