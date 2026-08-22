using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeReference] public Effect ManualTargetEffect { get; private set; } = null;
    [field: SerializeReference] public List<AutoTargetEffect> OtherEffects { get; private set; }
}
