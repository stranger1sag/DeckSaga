using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostReductionGA : GameAction
{
    public int Amount {get; private set;}
    public CostReductionGA(int amount)
    {
        Amount = amount;
    }
}
