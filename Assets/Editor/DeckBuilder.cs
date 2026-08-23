using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 设计稿卡池批量建卡工具。通过菜单触发，逐张生成 CardData 资产。
// 目标：把 Docs/GameDesign.md 第4部分的卡池（公共6张 + 四角色池）建成可运行的 CardData。
public static class DeckBuilder
{
    // ===== 效果工厂：构造 Effect 并按字段赋值（字段为 [SerializeField] private）=====
    public static DealDamageEffect Deal(int amount, int hits = 1, bool pierce = false)
    {
        var e = new DealDamageEffect();
        CardBuilder.SetEffectField(e, "damageAmount", amount);
        CardBuilder.SetEffectField(e, "hits", hits);
        CardBuilder.SetEffectField(e, "pierce", pierce);
        return e;
    }
    public static ArmorDamageEffect ArmorDmg(int baseDamage, float armorMult)
    {
        var e = new ArmorDamageEffect();
        CardBuilder.SetEffectField(e, "baseDamage", baseDamage);
        CardBuilder.SetEffectField(e, "armorMult", armorMult);
        return e;
    }
    public static AddStatusEffectEffect Status(StatusEffectType t, int stack)
    {
        var e = new AddStatusEffectEffect();
        CardBuilder.SetEffectField(e, "statusEffectType", t);
        CardBuilder.SetEffectField(e, "StackCount", stack);
        return e;
    }
    public static DrawCardsEffect Draw(int n) { var e = new DrawCardsEffect(); CardBuilder.SetEffectField(e, "drawAmount", n); return e; }
    public static HealEffect Heal(int n) { var e = new HealEffect(); CardBuilder.SetEffectField(e, "healAmount", n); return e; }
    public static GainManaEffect Mana(int n) { var e = new GainManaEffect(); CardBuilder.SetEffectField(e, "manaAmount", n); return e; }
    public static SacrificeEffect Sacrifice(int n) { var e = new SacrificeEffect(); CardBuilder.SetEffectField(e, "sacrificeAmount", n); return e; }
    public static CostReductionEffect Cost(int n) { var e = new CostReductionEffect(); CardBuilder.SetEffectField(e, "reduction", n); return e; }
    public static RemoveDebuffEffect RemoveDebuff() { return new RemoveDebuffEffect(); }

    // ===== 目标模式 =====
    public static readonly AllEnemiesTM AllEnemies = new();
    public static readonly HeroTM Hero = new();
    public static readonly NoTM None = new();
    public static readonly RandomEnemyTM RandomEnemy = new();

    // 构建一张卡：手动目标效果 + 若干自动目标效果
    public static void Build(string name, int mana, CardType type, string desc, string folder, Effect manual = null, List<(Effect, TargetMode)> autos = null, bool exhaust = false, bool retain = false)
    {
        string path = folder + "/" + name + ".asset";
        var existing = AssetDatabase.LoadAssetAtPath<CardData>(path);
        var data = existing != null ? existing : CardBuilder.CreateCard(name, mana, type, desc, null, folder);
        // 覆盖通用头字段（幂等）
        var so = new SerializedObject(data);
        var descP = so.FindProperty("<Description>k__BackingField"); if(descP != null) descP.stringValue = desc;
        var manaP = so.FindProperty("<Mana>k__BackingField"); if(manaP != null) manaP.intValue = mana;
        var typeP = so.FindProperty("<CardType>k__BackingField"); if(typeP != null) typeP.enumValueIndex = (int)type;
        var exP = so.FindProperty("<Exhaust>k__BackingField"); if(exP != null) exP.boolValue = exhaust;
        var retP = so.FindProperty("<Retain>k__BackingField"); if(retP != null) retP.boolValue = retain;
        so.ApplyModifiedProperties();

        // 清空旧效果并立即落地，避免后续新 SerializedObject 读到旧列表导致效果重复
        var other = so.FindProperty("<OtherEffects>k__BackingField");
        other.ClearArray();
        so.ApplyModifiedProperties();
        CardBuilder.SetManual(data, manual);
        if(autos != null)
            foreach(var (effect, tm) in autos)
                CardBuilder.AddAuto(data, effect, tm);
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    // 便捷重载：仅自动效果
    public static void Build(string name, int mana, CardType type, string desc, string folder, List<(Effect, TargetMode)> autos, bool exhaust = false, bool retain = false)
        => Build(name, mana, type, desc, folder, null, autos, exhaust, retain);
}
