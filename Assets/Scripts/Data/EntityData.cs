using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public GameObject projectilePrefab;
    public Color color;
    public List<Item> startingItems;

    public Stats stats;
}

[Serializable]
public struct Stats
{
    public int bulletPerSeconds;
    public int maxHealth;
    public int speed;
    public int damage;
    public float lifeSteal;
    public float bulletSpeed;

    public static Stats operator +(Stats _a,Stats _b)
    {
        return new Stats 
        {
            bulletPerSeconds = _a.bulletPerSeconds + _b.bulletPerSeconds,
            maxHealth = _a.maxHealth + _b.maxHealth,
            speed = _a.speed + _b.speed,
            damage = _a.damage + _b.damage, 
            bulletSpeed = _a.bulletSpeed + _b.bulletSpeed,
            lifeSteal = _a.lifeSteal + _b.lifeSteal,
        };
    }

    public static Stats operator -(Stats _a, Stats _b)
    {
        return new Stats
        {
            bulletPerSeconds = _a.bulletPerSeconds - _b.bulletPerSeconds,
            maxHealth = _a.maxHealth - _b.maxHealth,
            speed = _a.speed - _b.speed,
            damage = _a.damage - _b.damage,
            bulletSpeed = _a.bulletSpeed - _b.bulletSpeed,
            lifeSteal = _a.lifeSteal - _b.lifeSteal,
        };
    }
}
