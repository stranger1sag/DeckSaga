using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public int Mana {get; private set;}
    public Sprite Image => data.Image;
    public CardType Type => data.CardType;
    public bool Exhaust => data.Exhaust;
    public bool Retain => data.Retain;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    private readonly CardData data;
    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
    }
}
