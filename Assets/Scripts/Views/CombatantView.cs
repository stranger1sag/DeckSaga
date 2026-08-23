using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StatusEffectsUI statusEffectsUI;

    // 血条（位于 CombatantViewBase 预制体内的 HPBarCanvas 里的两个 UI Image）
    // 位置/锚点/高度都在预制体里配好，代码只负责动态宽度（对齐贴图宽度）
    [SerializeField] private UnityEngine.UI.Image hpBarBackground;
    [SerializeField] private UnityEngine.UI.Image hpBarFill;

    // 血条世界高度（与胶囊贴图原生高度一致，保持两端半圆）；宽度在 Setup 时按角色贴图算
    private const float HPBarHeight = 0.42f;
    private float hpBarWidth = 1.8f;

    // 杀戮尖塔式：暗底 + 红色血条
    private static readonly Color HPBarBgColor = new Color(0.08f, 0.09f, 0.1f, 0.92f);
    private static readonly Color HPBarFillColor = new Color(0.8f, 0.17f, 0.17f, 1f);

    public int MaxHealth {get; private set;}
    public int CurrentHealth {get; private set;}
    private readonly Dictionary<StatusEffectType,int> statusEffects = new();

    // 角色动画（挂在 sprite 的 GameObject 上，由 controller 播放 Idle/Walk/Attack/Hurt/Death）
    private Animator animator;

    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        ResolveHealthBar();
        // 宽度与角色贴图世界宽度一致（不硬编码，按实际算）
        if(spriteRenderer != null && spriteRenderer.sprite != null)
            hpBarWidth = spriteRenderer.sprite.bounds.size.x * spriteRenderer.transform.lossyScale.x;
        LayoutHealthBar();
        UpdateHealthUi();
    }

    // 解析血条引用；未连线时按子级名称 HPBarBG / HPBarFill 查找
    private void ResolveHealthBar()
    {
        if(hpBarBackground == null || hpBarFill == null)
        {
            foreach(UnityEngine.UI.Image img in GetComponentsInChildren<UnityEngine.UI.Image>(true))
            {
                if(img.gameObject.name == "HPBarBG") hpBarBackground = img;
                else if(img.gameObject.name == "HPBarFill") hpBarFill = img;
            }
        }
        if(hpBarBackground != null) hpBarBackground.color = HPBarBgColor;
        if(hpBarFill != null) hpBarFill.color = HPBarFillColor;
    }

    // 依据角色贴图宽度设置血条矩形宽度（高度固定，布局在预制体内）
    private void LayoutHealthBar()
    {
        if(hpBarBackground != null)
            hpBarBackground.rectTransform.sizeDelta = new Vector2(hpBarWidth, HPBarHeight);
        if(hpBarFill != null)
            hpBarFill.rectTransform.sizeDelta = new Vector2(hpBarWidth, HPBarHeight);
    }

    // 只更新动态部分：数字与填充比例（填充为 Filled，按胶囊形状从左往右裁剪，左端保持半圆）
    private void UpdateHealthUi()
    {
        healthText.text = CurrentHealth + "/" + MaxHealth;
        float ratio = MaxHealth > 0 ? Mathf.Clamp01((float)CurrentHealth / MaxHealth) : 0f;
        if(hpBarFill != null) hpBarFill.fillAmount = ratio;
    }

    // 播放指定动画状态；仅当存在 Animator 且该状态存在时才播放（怪物只有 Idle，避免报错）
    public void PlayAnimation(string stateName, float normalizedTime = 0f)
    {
        if(animator == null && spriteRenderer != null) animator = spriteRenderer.GetComponent<Animator>();
        if(animator != null && animator.HasState(0, Animator.StringToHash(stateName)))
            animator.Play(stateName, 0, normalizedTime);
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
        UpdateHealthUi();
        // 交给子类处理动画（英雄显示 Hurt/Death；怪物默认只有 Idle 不处理）
        if(CurrentHealth <= 0) OnDied();
        else OnDamaged();
    }

    // 子类可重写：受伤/死亡动画。默认无操作（怪物只有 Idle）
    protected virtual void OnDamaged() {}
    protected virtual void OnDied() {}
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
