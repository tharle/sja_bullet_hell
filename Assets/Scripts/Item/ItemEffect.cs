using System;
using UnityEngine;

[Serializable]
public abstract class ItemEffect
{
    [SerializeField] private string name;

    public abstract void Execute(Entity owner);
}

[Serializable]
public abstract class BulletEffect : ItemEffect
{
    public abstract void Execute(Entity owner, Projectile projectile = null);
}
