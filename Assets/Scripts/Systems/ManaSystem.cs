using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;
    private const int MAX_MANA = 3;
    private int currentMana = MAX_MANA;
    private int costDiscount = 0;   // 下一张牌的减免费用(用完即消耗)
    private void Start()
    {
        manaUI.UpdateMana(currentMana);
    }
    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.AttachPerformer<GainManaGA>(GainManaPerformer);
        ActionSystem.AttachPerformer<CostReductionGA>(CostReductionPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.DetachPerformer<GainManaGA>();
        ActionSystem.DetachPerformer<CostReductionGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }
    public bool HasEnoughMana(int amount)
    {
        return currentMana >= amount;
    }
    public void AddCostDiscount(int amount) { costDiscount += amount; }
    // 返回下一张牌的减免并消耗
    public int ConsumeCostDiscount()
    {
        int d = costDiscount;
        costDiscount = 0;
        return d;
    }
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        currentMana -= spendManaGA.Amount;
        manaUI.UpdateMana(currentMana);
        yield return null;
    }
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        currentMana = MAX_MANA;
        manaUI.UpdateMana(currentMana);
        yield return null;
    }
    private IEnumerator GainManaPerformer(GainManaGA gainManaGA)
    {
        currentMana = Mathf.Min(MAX_MANA, currentMana + gainManaGA.Amount);
        manaUI.UpdateMana(currentMana);
        yield return null;
    }
    private IEnumerator CostReductionPerformer(CostReductionGA costReductionGA)
    {
        costDiscount += costReductionGA.Amount;
        yield return null;
    }
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
