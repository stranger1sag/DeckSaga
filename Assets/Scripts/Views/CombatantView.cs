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
        // 宽度与角色实际可视宽度一致（用紧致alpha宽度去掉透明空白，避免Sprite外围空白把血条撑宽）
        if(spriteRenderer != null && spriteRenderer.sprite != null)
            hpBarWidth = GetTightWidth(spriteRenderer.sprite) * spriteRenderer.transform.lossyScale.x;
        LayoutHealthBar();
        UpdateHealthUi();
    }

    // 计算精灵的紧致alpha宽度（去掉透明空白），返回该帧在世界@scale1的宽度；不可读则退回 bounds
    private static float GetTightWidth(Sprite sprite)
    {
        Texture2D tex = sprite.texture;
        if(tex == null || !tex.isReadable) return sprite.bounds.size.x;
        Rect r = sprite.rect;
        int w = Mathf.Clamp((int)r.width, 1, 2048);
        int h = Mathf.Clamp((int)r.height, 1, 2048);
        var px = tex.GetPixels((int)r.x, (int)r.y, w, h);
        int minx = w, maxx = -1;
        for(int y = 0; y < h; y++)
            for(int x = 0; x < w; x++)
                if(px[y * w + x].a > 0.05f) { if(x < minx) minx = x; if(x > maxx) maxx = x; }
        if(maxx < 0) return sprite.bounds.size.x;
        float tight = (maxx - minx + 1) / sprite.pixelsPerUnit;
        return tight > 0 ? tight : sprite.bounds.size.x;
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
        // 受击击退：向远离攻击者方向（怪物在右→向右，英雄在左→向左）移动一小段，再回到原位
        Vector3 origPos = transform.position;
        float knockDir = Mathf.Sign(transform.position.x);
        if(knockDir == 0f) knockDir = 1f;
        transform.DOMoveX(origPos.x + knockDir * 0.6f, 0.12f).OnComplete(() => transform.DOMoveX(origPos.x, 0.12f));
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
