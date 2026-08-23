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
}
