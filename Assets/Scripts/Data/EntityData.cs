using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public GameObject ProjectilePrefab;
    public Color Color;
    public List<Item> StartingItems;
    public EBullet Bullet;

    public Stats stats;
}

[Serializable]
public struct Stats
{
    public int BulletPerSeconds;
    public int MaxHealth;
    public float Speed;
    public int Damage;
    public float LifeSteal;
    public float BulletSpeed;

    public static Stats operator +(Stats _a,Stats _b)
    {
        return new Stats 
        {
            BulletPerSeconds = _a.BulletPerSeconds + _b.BulletPerSeconds,
            MaxHealth = _a.MaxHealth + _b.MaxHealth,
            Speed = _a.Speed + _b.Speed,
            Damage = _a.Damage + _b.Damage, 
            BulletSpeed = _a.BulletSpeed + _b.BulletSpeed,
            LifeSteal = _a.LifeSteal + _b.LifeSteal,
        };
    }

    public static Stats operator -(Stats _a, Stats _b)
    {
        return new Stats
        {
            BulletPerSeconds = _a.BulletPerSeconds - _b.BulletPerSeconds,
            MaxHealth = _a.MaxHealth - _b.MaxHealth,
            Speed = _a.Speed - _b.Speed,
            Damage = _a.Damage - _b.Damage,
            BulletSpeed = _a.BulletSpeed - _b.BulletSpeed,
            LifeSteal = _a.LifeSteal - _b.LifeSteal,
        };
    }
}
