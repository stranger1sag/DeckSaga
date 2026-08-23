using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 程序化生成 CardData 资产（处理 [SerializeReference] 效果树）的工具
public static class CardBuilder
{
    public static CardData CreateCard(string name, int mana, CardType type, string desc, Sprite img, string folder)
    {
        var data = ScriptableObject.CreateInstance<CardData>();
        data.name = name;
        AssetDatabase.CreateAsset(data, folder + "/" + name + ".asset");
        var so = new SerializedObject(data);
        var descP = so.FindProperty("<Description>k__BackingField"); if(descP!=null) descP.stringValue = desc;
        var manaP = so.FindProperty("<Mana>k__BackingField"); if(manaP!=null) manaP.intValue = mana;
        var typeP = so.FindProperty("<CardType>k__BackingField"); if(typeP!=null) typeP.enumValueIndex = (int)type;
        var imgP = so.FindProperty("<Image>k__BackingField"); if(imgP!=null) imgP.objectReferenceValue = img;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        return data;
    }

    // 设置自动目标效果：OtherEffects.Add( AutoTargetEffect(effect, targetMode) )
    public static void AddAuto(CardData data, Effect effect, TargetMode targetMode)
    {
        var so = new SerializedObject(data);
        var list = so.FindProperty("<OtherEffects>k__BackingField");
        if(list == null) return;
        int idx = list.arraySize;
        list.InsertArrayElementAtIndex(idx);
        var elem = list.GetArrayElementAtIndex(idx);
        var ate = new AutoTargetEffect();
        SetRefTree(ate, "Effect", effect);
        SetRefTree(ate, "TargetMode", targetMode);
        elem.managedReferenceValue = ate;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    // 设置手动目标效果
    public static void SetManual(CardData data, Effect effect)
    {
        var so = new SerializedObject(data);
        var prop = so.FindProperty("<ManualTargetEffect>k__BackingField");
        if(prop != null) prop.managedReferenceValue = effect;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
    }

    // 通过反射设置嵌套 [SerializeReference] 字段（effect 的对象值）
    private static void SetRefTree(object root, string propName, object value)
    {
        var f = root.GetType().GetField(propName);
        if(f != null) f.SetValue(root, value);
    }

    // 用反射设置 Effect 的具体字段（deal damage 的 amount / addstatus 的 type+stack 等）
    public static void SetEffectField(object effect, string field, object value)
    {
        var f = effect.GetType().GetField(field);
        if(f != null) f.SetValue(effect, value);
    }
}
