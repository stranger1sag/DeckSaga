using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 设计稿卡池内容表：菜单触发 → 逐张生成 CardData。
// 参考 Docs/GameDesign.md 第4部分。
public static class DeckContent
{
    const string Common = "Assets/Data/Cards/Common";
    const string Soldier = "Assets/Data/Cards/Soldier";
    const string Orc = "Assets/Data/Cards/Orc";
    const string Blood = "Assets/Data/Cards/Blood";
    const string Demon = "Assets/Data/Cards/Demon";

    [MenuItem("Tools/DeckSaga/Build Common Pool")]
    public static void BuildCommon()
    {
        // 公共池（所有角色通用）
        DeckBuilder.Build("打击", 1, CardType.ATTACK, "造成 6 点伤害。", Common, DeckBuilder.Deal(6));
        DeckBuilder.Build("防御", 1, CardType.SKILL, "获得 5 点格挡。", Common, new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,5), DeckBuilder.Hero) });
        DeckBuilder.Build("重击", 2, CardType.ATTACK, "造成 10 点伤害。", Common, DeckBuilder.Deal(10));
        DeckBuilder.Build("铁壁", 1, CardType.SKILL, "获得 8 点格挡。", Common, new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,8), DeckBuilder.Hero) });
        DeckBuilder.Build("战吼", 1, CardType.SKILL, "抽 2 张牌。", Common, new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(2), DeckBuilder.None) });
        DeckBuilder.Build("急救", 1, CardType.SKILL, "回复 3 点生命。", Common, new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(3), DeckBuilder.Hero) });
        AssetDatabase.SaveAssets();
        Debug.Log("[DeckBuilder] Common 池构建完成");
    }

    [MenuItem("Tools/DeckSaga/Build Soldier Deck")]
    public static void BuildSoldier()
    {
        // §3.1 铁壁士兵（不含重复的公共6张）
        DeckBuilder.Build("坚守", 2, CardType.SKILL, "获得 14 点格挡，抽 1 张牌。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,14), DeckBuilder.Hero), (DeckBuilder.Draw(1), DeckBuilder.None) });
        DeckBuilder.Build("磐石", 1, CardType.SKILL, "获得 2 点敏捷（本回合每得格挡 +2）。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.DEXTERITY,2), DeckBuilder.Hero) });
        DeckBuilder.Build("无懈可击", 2, CardType.SKILL, "获得 10 点格挡（不随回合清零）。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,10), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.WARD,1), DeckBuilder.Hero) });
        DeckBuilder.Build("荆棘", 1, CardType.SKILL, "获得 6 点荆棘（受击时反击敌人）。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.THORNS,6), DeckBuilder.Hero) });
        DeckBuilder.Build("反震", 1, CardType.SKILL, "本回合受击反击 4 点。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.THORNS,4), DeckBuilder.Hero) });
        DeckBuilder.Build("盾击", 1, CardType.ATTACK, "造成 6 点伤害，并使目标易伤。", Soldier,
            DeckBuilder.Deal(6), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.VULNERABLE,1), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("破势", 1, CardType.ATTACK, "造成 4 点伤害；目标本回合受到伤害 +50%。", Soldier,
            DeckBuilder.Deal(4), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.VULNERABLE,1), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("横刀", 2, CardType.ATTACK, "对所有敌人造成 5 点伤害。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Deal(5), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("反击指令", 1, CardType.SKILL, "获得 3 点格挡，抽 1 张牌。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,3), DeckBuilder.Hero), (DeckBuilder.Draw(1), DeckBuilder.None) });
        DeckBuilder.Build("排兵", 1, CardType.SKILL, "下一张牌费用 -1。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Cost(1), DeckBuilder.None) });
        DeckBuilder.Build("军号", 1, CardType.SKILL, "抽 2 张牌，获得 2 点格挡。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(2), DeckBuilder.None), (DeckBuilder.Status(StatusEffectType.ARMOR,2), DeckBuilder.Hero) });
        DeckBuilder.Build("破敌", 2, CardType.ATTACK, "造成 8 点伤害，无视格挡。", Soldier, DeckBuilder.Deal(8, 1, true));
        DeckBuilder.Build("老兵愈伤", 1, CardType.SKILL, "回复 4 点生命。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(4), DeckBuilder.Hero) });
        DeckBuilder.Build("盾墙", 2, CardType.SKILL, "获得 24 点格挡（消耗）。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,24), DeckBuilder.Hero) }, exhaust: true);
        DeckBuilder.Build("铁壁长城", 3, CardType.SKILL, "获得 30 点格挡（不随回合清零，消耗）。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,30), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.WARD,1), DeckBuilder.Hero) }, exhaust: true);
        DeckBuilder.Build("荣耀反击", 2, CardType.SKILL, "本回合每受一次攻击反击 8 点。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.THORNS,8), DeckBuilder.Hero) });
        DeckBuilder.Build("持久战意", 2, CardType.POWER, "本场每回合开始获得 2 点格挡。", Soldier,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.PER_TURN_ARMOR,2), DeckBuilder.Hero) });
        AssetDatabase.SaveAssets();
        Debug.Log("[DeckBuilder] Soldier 池构建完成");
    }

    [MenuItem("Tools/DeckSaga/Build Orc Deck")]
    public static void BuildOrc()
    {
        // §3.2 狂怒兽人（不含重复的公共6张）
        DeckBuilder.Build("重斩", 2, CardType.ATTACK, "造成 12 点伤害。", Orc, DeckBuilder.Deal(12));
        DeckBuilder.Build("咆哮", 1, CardType.SKILL, "获得 2 点力量。", Orc, new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.STRENGTH,2), DeckBuilder.Hero) });
        DeckBuilder.Build("力量挥击", 1, CardType.ATTACK, "造成 4 点伤害，获得 2 点力量。", Orc,
            DeckBuilder.Deal(4), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.STRENGTH,2), DeckBuilder.Hero) });
        DeckBuilder.Build("血性", 0, CardType.SKILL, "损失 2 点生命，获得 2 点格挡。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(2), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.ARMOR,2), DeckBuilder.Hero) });
        DeckBuilder.Build("血怒", 1, CardType.SKILL, "损失 3 点生命，获得 3 点力量。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(3), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.STRENGTH,3), DeckBuilder.Hero) });
        DeckBuilder.Build("裂地", 2, CardType.ATTACK, "造成 4×2 点伤害。", Orc, DeckBuilder.Deal(4, 2));
        DeckBuilder.Build("疯狗连咬", 1, CardType.ATTACK, "造成 2×3 点伤害。", Orc, DeckBuilder.Deal(2, 3));
        DeckBuilder.Build("连斩", 1, CardType.ATTACK, "造成 3×2 点伤害。", Orc, DeckBuilder.Deal(3, 2));
        DeckBuilder.Build("碎骨", 1, CardType.ATTACK, "造成 5 点伤害，并使目标易伤。", Orc,
            DeckBuilder.Deal(5), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.VULNERABLE,1), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("破甲", 1, CardType.ATTACK, "造成 6 点伤害，无视格挡。", Orc, DeckBuilder.Deal(6, 1, true));
        DeckBuilder.Build("蛮力", 2, CardType.SKILL, "损失 4 点生命，获得 1 点能量。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(4), DeckBuilder.Hero), (DeckBuilder.Mana(1), DeckBuilder.None) });
        DeckBuilder.Build("强健", 1, CardType.SKILL, "回复 4 点生命。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(4), DeckBuilder.Hero) });
        DeckBuilder.Build("抽牌", 1, CardType.SKILL, "抽 2 张牌。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(2), DeckBuilder.None) });
        DeckBuilder.Build("斗鱼猛攻", 2, CardType.ATTACK, "造成 2×3 点伤害，抽 1 张牌。", Orc,
            DeckBuilder.Deal(2, 3), new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(1), DeckBuilder.None) });
        DeckBuilder.Build("狂野冲撞", 2, CardType.ATTACK, "造成 8 点伤害（力量>3 时 +6）。", Orc, DeckBuilder.Deal(8));
        DeckBuilder.Build("怒不可遏", 2, CardType.SKILL, "本回合每张攻击牌+4 伤害。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.FURY,4), DeckBuilder.Hero) });
        DeckBuilder.Build("血勇", 1, CardType.ATTACK, "造成 6 点伤害。", Orc, DeckBuilder.Deal(6));
        DeckBuilder.Build("兽人本能", 1, CardType.SKILL, "抽 2 张牌，获得 1 点力量。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(2), DeckBuilder.None), (DeckBuilder.Status(StatusEffectType.STRENGTH,1), DeckBuilder.Hero) });
        DeckBuilder.Build("嗜血渴望", 1, CardType.SKILL, "损失 2 点生命，下张攻击牌+4 伤害。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(2), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.NEXT_ATTACK_BONUS,4), DeckBuilder.Hero) });
        DeckBuilder.Build("狂暴化", 3, CardType.SKILL, "本场力量 +5（消耗）。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.STRENGTH,5), DeckBuilder.Hero) }, exhaust: true);
        DeckBuilder.Build("嗜血战刃", 3, CardType.ATTACK, "每点力量造成 2 点伤害，打全体。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.StrengthDmg(0, 2), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("万兽狂潮", 2, CardType.ATTACK, "造成 2×(力量+3) 点伤害。", Orc, DeckBuilder.StrengthDmg(6, 2));
        DeckBuilder.Build("狂血沸腾", 2, CardType.POWER, "本场每回合开始获得 1 点力量。", Orc,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.PER_TURN_STRENGTH,1), DeckBuilder.Hero) });
        AssetDatabase.SaveAssets();
        Debug.Log("[DeckBuilder] Orc 池构建完成");
    }

    [MenuItem("Tools/DeckSaga/Build Blood Deck")]
    public static void BuildBlood()
    {
        // §3.3 嗜血者（不含重复的公共6张）
        DeckBuilder.Build("血袭", 1, CardType.ATTACK, "造成 6 点伤害，回复 3 点生命。", Blood,
            DeckBuilder.Deal(6), new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(3), DeckBuilder.Hero) });
        DeckBuilder.Build("血潮", 2, CardType.ATTACK, "造成 8 点伤害，回复 6 点生命。", Blood,
            DeckBuilder.Deal(8), new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(6), DeckBuilder.Hero) });
        DeckBuilder.Build("生命虹吸", 2, CardType.ATTACK, "对所有敌人造成 5 点伤害，按敌数回复生命。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Deal(5), DeckBuilder.AllEnemies), (DeckBuilder.Heal(3), DeckBuilder.Hero) });
        DeckBuilder.Build("凝血", 1, CardType.SKILL, "获得 6 点格挡，回复 2 点生命。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,6), DeckBuilder.Hero), (DeckBuilder.Heal(2), DeckBuilder.Hero) });
        DeckBuilder.Build("毒刃", 1, CardType.ATTACK, "造成 4 点伤害，施 2 层中毒。", Blood,
            DeckBuilder.Deal(4), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.POISON,2), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("剧毒", 1, CardType.SKILL, "施 3 层中毒。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.POISON,3), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("毒性爆发", 2, CardType.SKILL, "对所有敌人施 5 层中毒。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.POISON,5), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("血祭", 1, CardType.SKILL, "损失 4 点生命，抽 2 张牌。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(4), DeckBuilder.Hero), (DeckBuilder.Draw(2), DeckBuilder.None) });
        DeckBuilder.Build("血之献祭", 2, CardType.SKILL, "损失 6 点生命，获得 2 点能量。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(6), DeckBuilder.Hero), (DeckBuilder.Mana(2), DeckBuilder.None) });
        DeckBuilder.Build("放血", 1, CardType.SKILL, "损失 5 点生命，抽 3 张牌。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Sacrifice(5), DeckBuilder.Hero), (DeckBuilder.Draw(3), DeckBuilder.None) });
        DeckBuilder.Build("吸血本能", 1, CardType.ATTACK, "造成 4 点伤害，回复 2 点生命。", Blood,
            DeckBuilder.Deal(4), new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(2), DeckBuilder.Hero) });
        DeckBuilder.Build("濒死爆发", 2, CardType.ATTACK, "造成 6 点伤害；自身生命<50% 时 +10。", Blood,
            DeckBuilder.Cond(6, 10, ConditionalDamageEffect.Condition.CASTER_HP_PCT_LOW, 50));
        DeckBuilder.Build("血之渴望", 1, CardType.ATTACK, "造成 5 点伤害，回复 2 点生命。", Blood,
            DeckBuilder.Deal(5), new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(2), DeckBuilder.Hero) });
        DeckBuilder.Build("血之屏障", 1, CardType.SKILL, "获得 5 点格挡，回复 1 点生命。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,5), DeckBuilder.Hero), (DeckBuilder.Heal(1), DeckBuilder.Hero) });
        DeckBuilder.Build("血色祈祷", 2, CardType.SKILL, "回复 7 点生命。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(7), DeckBuilder.Hero) });
        DeckBuilder.Build("血族之躯", 1, CardType.SKILL, "回复 5 点生命，获得 1 点力量。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(5), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.STRENGTH,1), DeckBuilder.Hero) });
        DeckBuilder.Build("血勇", 1, CardType.ATTACK, "造成 6 点伤害。", Blood, DeckBuilder.Deal(6));
        DeckBuilder.Build("终极吸食", 2, CardType.ATTACK, "造成 10 点伤害，回复等量生命（消耗）。", Blood,
            DeckBuilder.Deal(10), new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(10), DeckBuilder.Hero) }, exhaust: true);
        DeckBuilder.Build("血之契约", 2, CardType.POWER, "本场每次命中敌人额外 +1 吸血。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.EXTRA_LIFESTEAL,1), DeckBuilder.Hero) });
        DeckBuilder.Build("剔伐", 2, CardType.SKILL, "删除一张手牌，回复 6 点生命。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.RemoveFromDeck(1), DeckBuilder.None), (DeckBuilder.Heal(6), DeckBuilder.Hero) });
        DeckBuilder.Build("精血", 1, CardType.SKILL, "删除一张牌，抽 2 张牌。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.RemoveFromDeck(1), DeckBuilder.None), (DeckBuilder.Draw(2), DeckBuilder.None) });
        DeckBuilder.Build("血液凝滞", 1, CardType.SKILL, "本回合免受自伤。", Blood,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.NO_SELF_DAMAGE,1), DeckBuilder.Hero) });
        AssetDatabase.SaveAssets();
        Debug.Log("[DeckBuilder] Blood 池构建完成");
    }

    [MenuItem("Tools/DeckSaga/Build Demon Deck")]
    public static void BuildDemon()
    {
        // §3.4 深渊恶魔（不含重复的公共6张）
        DeckBuilder.Build("烈焰术", 1, CardType.ATTACK, "造成 6 点伤害，施 2 层燃烧。", Demon,
            DeckBuilder.Deal(6), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.BURN,2), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("火种", 1, CardType.SKILL, "施 2 层燃烧，抽 1 张牌。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.BURN,2), DeckBuilder.RandomEnemy), (DeckBuilder.Draw(1), DeckBuilder.None) });
        DeckBuilder.Build("余烬", 0, CardType.SKILL, "施 1 层燃烧。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.BURN,1), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("燎原", 2, CardType.ATTACK, "造成 3 点伤害，施 3 层燃烧。", Demon,
            DeckBuilder.Deal(3), new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.BURN,3), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("燃烧殆尽", 2, CardType.SKILL, "对所有敌人施 4 层燃烧。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.BURN,4), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("虚弱诅咒", 1, CardType.SKILL, "使目标虚弱 1。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.WEAK,1), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("破势一击", 1, CardType.ATTACK, "造成 5 点伤害，无视格挡。", Demon, DeckBuilder.Deal(5, 1, true));
        DeckBuilder.Build("处决", 2, CardType.ATTACK, "目标生命<20 直接斩杀，否则造成 8 点伤害。", Demon,
            DeckBuilder.Execute(8, 20));
        DeckBuilder.Build("刀刃", 2, CardType.ATTACK, "目标生命<30 直接斩杀。", Demon,
            DeckBuilder.Execute(0, 30));
        DeckBuilder.Build("火球术", 1, CardType.ATTACK, "造成 7 点伤害。", Demon, DeckBuilder.Deal(7));
        DeckBuilder.Build("连珠火", 1, CardType.ATTACK, "造成 3×2 点伤害。", Demon, DeckBuilder.Deal(3, 2));
        DeckBuilder.Build("灰烬风暴", 2, CardType.ATTACK, "对所有敌人造成 4 点伤害。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Deal(4), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("抽牌火", 1, CardType.SKILL, "抽 2 张牌。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Draw(2), DeckBuilder.None) });
        DeckBuilder.Build("火之护盾", 1, CardType.SKILL, "获得 5 点格挡，施 2 层燃烧。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,5), DeckBuilder.Hero), (DeckBuilder.Status(StatusEffectType.BURN,2), DeckBuilder.RandomEnemy) });
        DeckBuilder.Build("灼热核心", 1, CardType.ATTACK, "造成 6 点伤害；目标燃烧>2 时 +6。", Demon,
            DeckBuilder.Cond(6, 6, ConditionalDamageEffect.Condition.TARGET_BURN_GT, 2));
        DeckBuilder.Build("熔岩护体", 1, CardType.SKILL, "获得 4 点格挡。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.ARMOR,4), DeckBuilder.Hero) });
        DeckBuilder.Build("恶魔之翼", 1, CardType.ATTACK, "造成 6 点伤害。", Demon, DeckBuilder.Deal(6));
        DeckBuilder.Build("火焰补益", 1, CardType.SKILL, "回复 3 点生命，抽 1 张牌。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Heal(3), DeckBuilder.Hero), (DeckBuilder.Draw(1), DeckBuilder.None) });
        DeckBuilder.Build("咒能过载", 1, CardType.SKILL, "获得 1 点能量，损失 2 点生命。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Mana(1), DeckBuilder.None), (DeckBuilder.Sacrifice(2), DeckBuilder.Hero) });
        DeckBuilder.Build("灼烧血脉", 1, CardType.SKILL, "获得 1 点力量。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.STRENGTH,1), DeckBuilder.Hero) });
        DeckBuilder.Build("火刑", 2, CardType.ATTACK, "造成 8 点伤害；目标燃烧叠3 时斩杀。", Demon, DeckBuilder.Deal(8));
        DeckBuilder.Build("焚尽", 3, CardType.SKILL, "对所有敌人造成 16 点伤害，清空燃烧层数。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Deal(16), DeckBuilder.AllEnemies), (DeckBuilder.ClearBurn(), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("无尽火雨", 2, CardType.ATTACK, "造成 2×(全场燃烧) 点伤害。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.BurnScaled(0, 2), DeckBuilder.AllEnemies) });
        DeckBuilder.Build("灼热流转", 2, CardType.POWER, "本场每回合开始给所有敌人 +1 燃烧。", Demon,
            new List<(Effect,TargetMode)>{ (DeckBuilder.Status(StatusEffectType.PER_TURN_BURN,1), DeckBuilder.Hero) });
        AssetDatabase.SaveAssets();
        Debug.Log("[DeckBuilder] Demon 池构建完成");
    }
}
