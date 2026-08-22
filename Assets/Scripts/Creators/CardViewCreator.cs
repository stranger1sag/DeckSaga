using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.transform.localScale = Vector3.zero;
        // 卡牌缩放读取预制体自身的 scale，尺寸在 CardView.prefab 里配置
        cardView.transform.DOScale(cardViewPrefab.transform.localScale, 0.15f);
        cardView.Setup(card);
        return cardView;
    }
}
