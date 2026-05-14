using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;
    [SerializeField] private Sprite armorSprite, burnSprite;
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
        return type switch
        {
            StatusEffectType.ARMOR => armorSprite,
            StatusEffectType.BURN => burnSprite,
            _ => null
        };
    }
}
