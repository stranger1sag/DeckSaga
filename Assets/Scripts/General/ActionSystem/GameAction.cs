using System.Collections.Generic;
using UnityEngine;
public abstract class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new(); //游戏动作前的反应
    public List<GameAction> PerformReactions { get; private set; } = new();//游戏动作执行时的反应
    public List<GameAction> PostReactions { get; private set; } = new();//游戏动作后的反应
}