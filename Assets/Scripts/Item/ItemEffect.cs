using System;
using UnityEngine;

[Serializable]
public abstract class ItemEffect
{
    [SerializeField] private string m_Name;
    [SerializeField] private string m_Description;

    public abstract void Execute(Entity owner);
}

[Serializable]
public abstract class BulletEffect : ItemEffect
{
    public abstract void Execute(Entity owner, Projectile projectile = null);
}
