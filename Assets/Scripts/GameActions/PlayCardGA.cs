using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{
    public EnemyView ManualTarget { get; set; }
    public Card Card{get; set;}
    public PlayCardGA(Card card)
    {
        Card = card;
    }
    public PlayCardGA(Card card, EnemyView manualTarget)
    {
        Card = card;
        ManualTarget = manualTarget;
    }
}
