using System;
using UnityEngine;

[Serializable]
public abstract class ItemEffect
{
    [SerializeField] private string name;

    public abstract void Execute(Entity _owner);
}

[Serializable]
public abstract class BulletEffect : ItemEffect
{
    public abstract void Execute(Entity _owner, Projectile _projectile = null);
}
