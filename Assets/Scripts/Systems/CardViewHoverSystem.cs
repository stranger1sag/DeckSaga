using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;
    public void Show(Card card, Vector3 position)
    {
        cardViewHover.Setup(card);
        cardViewHover.transform.position = position;
        cardViewHover.gameObject.SetActive(true);
    }

    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }
}
