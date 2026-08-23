using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;
    [SerializeField] private Sprite armorSprite, burnSprite;
    [SerializeField] private Sprite strengthSprite, dexteritySprite, thornsSprite, lifestealSprite, furySprite, wardSprite;
    [SerializeField] private Sprite vulnerableSprite, weakSprite, poisonSprite, stunSprite, tauntSprite, armorBreakSprite, depleteSprite;
    [SerializeField] private Sprite healSprite;
    private Dictionary<StatusEffectType, StatusEffectUI> statusEffectUIs = new();
    public void UpdateStatusEffectUI(StatusEffectType type, int stackCount)
    {
        if(stackCount ==0)
        {
            if (statusEffectUIs.ContainsKey(type))
            {
                StatusEffectUI statusEffectUI = statusEffectUIs[type];
                statusEffectUIs.Remove(type);
                Destroy(statusEffectUI.gameObject);
            }
        }
        else
        {
            if (!statusEffectUIs.ContainsKey(type))
            {
                StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, transform);
                statusEffectUIs.Add(type, statusEffectUI);
            }
            Sprite sprite = GetSpriteByType(type);
            statusEffectUIs[type].Setup(sprite, stackCount, GetColorByType(type));
        }
    }
    private Sprite GetSpriteByType(StatusEffectType type)
    {
        switch(type)
        {
            case StatusEffectType.ARMOR: return ResolveIcon(armorSprite, "armor");
            case StatusEffectType.BURN: return ResolveIcon(burnSprite, "burn");
            case StatusEffectType.STRENGTH: return ResolveIcon(strengthSprite, "strength");
            case StatusEffectType.DEXTERITY: return ResolveIcon(dexteritySprite, "dexterity");
            case StatusEffectType.THORNS: return ResolveIcon(thornsSprite, "thorns");
            case StatusEffectType.LIFESTEAL: return ResolveIcon(lifestealSprite, "lifesteal");
            case StatusEffectType.FURY: return ResolveIcon(furySprite, "energy");
            case StatusEffectType.WARD: return ResolveIcon(wardSprite, "ward");
            case StatusEffectType.VULNERABLE: return ResolveIcon(vulnerableSprite, "vulnerable");
            case StatusEffectType.WEAK: return ResolveIcon(weakSprite, "weak");
            case StatusEffectType.POISON: return ResolveIcon(poisonSprite, "poison");
            case StatusEffectType.STUN: return ResolveIcon(stunSprite, "stun");
            case StatusEffectType.TAUNT: return ResolveIcon(tauntSprite, "taunt");
            case StatusEffectType.ARMOR_BREAK: return ResolveIcon(armorBreakSprite, "armor_break");
            case StatusEffectType.DEPLETE: return ResolveIcon(depleteSprite, "deplete");
            default: return ResolveIcon(healSprite, "heal");
        }
    }
    // 字段未赋值时用 Resources.Load 兜底（运行时也能拿到图标）
    private Sprite ResolveIcon(Sprite assigned, string name)
    {
        if(assigned != null) return assigned;
        return Resources.Load<Sprite>("status_icons/" + name);
    }
    // 每种状态的图标颜色（白色图标 x 颜色 = 彩色）
    private Color GetColorByType(StatusEffectType type)
    {
        return type switch
        {
            StatusEffectType.ARMOR => new Color(0.55f, 0.75f, 1f),        // 蓝(防御)
            StatusEffectType.BURN => new Color(1f, 0.55f, 0.15f),         // 橙(燃烧)
            StatusEffectType.STRENGTH => new Color(1f, 0.4f, 0.4f),       // 红(力量)
            StatusEffectType.DEXTERITY => new Color(0.6f, 1f, 0.5f),      // 绿(敏捷)
            StatusEffectType.THORNS => new Color(0.6f, 1f, 0.35f),        // 黄绿(荆棘)
            StatusEffectType.LIFESTEAL => new Color(1f, 0.35f, 0.55f),    // 粉红(吸血)
            StatusEffectType.FURY => new Color(1f, 0.7f, 0.25f),          // 橙黄(怒气)
            StatusEffectType.WARD => new Color(0.4f, 0.6f, 1f),           // 深蓝(护体)
            StatusEffectType.VULNERABLE => new Color(1f, 0.85f, 0.4f),    // 黄(易伤)
            StatusEffectType.WEAK => new Color(0.7f, 0.7f, 0.7f),         // 灰(虚弱)
            StatusEffectType.POISON => new Color(0.5f, 0.85f, 0.35f),     // 绿(中毒)
            StatusEffectType.STUN => new Color(1f, 0.9f, 0.45f),          // 黄(眩晕)
            StatusEffectType.TAUNT => new Color(1f, 0.45f, 0.3f),         // 红橙(嘲讽)
            StatusEffectType.ARMOR_BREAK => new Color(1f, 0.6f, 0.35f),   // 橙(破甲)
            StatusEffectType.DEPLETE => new Color(0.75f, 0.6f, 0.9f),     // 紫(枯竭)
            _ => new Color(0.55f, 1f, 0.6f)                               // 绿(治疗)
        };
    }
}

