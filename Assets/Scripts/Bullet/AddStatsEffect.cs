using System;
using UnityEngine;

[Serializable]
public class AddStatsEffect : ItemEffect
{
    [SerializeField] public Stats statsToAdd;

    public override void Execute(Entity _owner)
    {
        _owner.AddStats(statsToAdd);
    }
}

[Serializable]
public class RemoveStatsEffect : ItemEffect
{
    [SerializeField] public Stats statsToRemove;

    public override void Execute(Entity _owner)
    {
        _owner.RemoveStats(statsToRemove);
    }
}
