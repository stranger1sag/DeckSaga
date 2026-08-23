using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public CardType CardType { get; private set; } = CardType.ATTACK;
    [field: SerializeField] public bool Exhaust { get; private set; }        // 消耗：用后本场消失
    [field: SerializeField] public bool Retain { get; private set; }        // 保留：回合结束留手
    [field: SerializeReference] public Effect ManualTargetEffect { get; private set; } = null;
    [field: SerializeReference] public List<AutoTargetEffect> OtherEffects { get; private set; }
}
