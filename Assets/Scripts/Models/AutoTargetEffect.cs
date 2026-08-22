using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoTargetEffect
{
    [field: SerializeReference] public Effect Effect { get; private set; }
    [field: SerializeReference] public TargetMode TargetMode { get; private set; }
}
