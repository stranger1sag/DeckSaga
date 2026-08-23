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
            statusEffectUIs[type].Setup(sprite, stackCount);
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
}
