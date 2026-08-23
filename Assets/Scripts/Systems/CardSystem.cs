using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    // 出牌事件：一张牌被打出时触发（供"出牌光环"类效果响应）
    public static event System.Action<Card, CombatantView> OnCardPlayed;
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> exhaustPile = new();
    private readonly List<Card> hand = new();
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    //publics
    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }

    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if(notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }
    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        Card playedCard = playCardGA.Card;
        hand.Remove(playedCard);
        CardView cardView = handView.RemoveCard(playedCard);

        // 保留 Retain：卡回到手上（仍然执行效果）；否则进弃牌堆/消耗堆
        if(playedCard.Retain)
        {
            hand.Add(playedCard);
            yield return handView.AddCard(cardView);
        }
        else
        {
            yield return DiscardCard(cardView, playedCard.Exhaust);
        }

        // 只有攻击牌(ATTACK)才让英雄播放攻击动作；技能/能力牌不播
        if(playCardGA.Card.Type == CardType.ATTACK && HeroSystem.Instance != null && HeroSystem.Instance.HeroView != null)
            HeroSystem.Instance.HeroView.PlayAttack();

        // 触发出牌事件（出牌光环效果响应）
        if(OnCardPlayed != null && HeroSystem.Instance != null && HeroSystem.Instance.HeroView != null)
            OnCardPlayed(playedCard, HeroSystem.Instance.HeroView);

        // 出牌费用(应用下一张牌的费用减免)
        int cost = Mathf.Max(0, playCardGA.Card.Mana - (ManaSystem.Instance != null ? ManaSystem.Instance.ConsumeCostDiscount() : 0));
        SpendManaGA spendManaGA = new(cost);
        ActionSystem.Instance.AddReaction(spendManaGA);
        if(playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.Card.ManualTargetEffect, new(){playCardGA.ManualTarget});
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGA = new(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    //helpers
    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card,drawPilePoint.position,drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }
    private IEnumerator DiscardCard(CardView cardView, bool exhaust = false)
    {
        if(exhaust) exhaustPile.Add(cardView.Card);
        else discardPile.Add(cardView.Card);
        cardView.transform.DOScale(Vector3.zero,0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}
