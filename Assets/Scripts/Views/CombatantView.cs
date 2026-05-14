using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;
    public int MaxHealth {get; private set;}
    public int CurrentHealth {get; private set;}
    private Dictionary<StatusEffectType,int> statusEffects = new();
    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth.ToString();
    }

    public void Damage(int damageAmount)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.ARMOR);
        if (currentArmor > 0)
        {
            if(currentArmor >= remainingDamage)
            {
                RemoveStatusEffect(StatusEffectType.ARMOR, remainingDamage);
                remainingDamage = 0;
            }
            else if(currentArmor < remainingDamage)
            {
                RemoveStatusEffect(StatusEffectType.ARMOR, currentArmor);
                remainingDamage -= currentArmor;
            }
        }
        CurrentHealth -= remainingDamage;
        if(CurrentHealth < 0) CurrentHealth = 0;
        transform.DOShakePosition(0.2f,0.5f);
        UpdateHealthText();
    }
    public void AddStatusEffect(StatusEffectType statusEffectType, int stackCount)
    {
        if(statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] += stackCount;
        }
        else
        {
            statusEffects.Add(statusEffectType, stackCount);
        }
        statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));
    }
    public void RemoveStatusEffect(StatusEffectType statusEffectType,int stackCount)
    {
        if(statusEffects.ContainsKey(statusEffectType))
        {
            statusEffects[statusEffectType] -= stackCount;
            if(statusEffects[statusEffectType] <= 0)
            {
                statusEffects.Remove(statusEffectType);
            }
            statusEffectsUI.UpdateStatusEffectUI(statusEffectType, GetStatusEffectStacks(statusEffectType));
        }
    }
    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    {
        if (statusEffects.ContainsKey(statusEffectType)) return statusEffects[statusEffectType];
        else return 0;
    }
}
