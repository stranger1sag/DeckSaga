using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 从手牌/牌库移除一张牌（删牌机制）
public class RemoveFromDeckGA : GameAction
{
    public int Count {get; private set;}
    public RemoveFromDeckGA(int count)
    {
        Count = count;
    }
}
