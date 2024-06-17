using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable, IContainer
{
    public Action OnDead;
    public Action OnHit;
    public Action OnHeal;
    public Action OnShoot;

    [Header("Entity")]
    [SerializeField] private EntityData data;
    protected List<Item> items = new List<Item>();
    protected Stats currentStats;
    protected int currentHealth;

    public GameObject BulletPrefab => data.projectilePrefab;
    public Stats Stats => currentStats;
    public Color EntityColor => data.color;
    public int CurrentHealth => currentHealth;
    public List<Item> StartingItems => data.startingItems;
    public List<Item> Items { get => items; set { } }

    protected virtual void Awake()
    {
        currentStats = data.stats;
        currentHealth = currentStats.maxHealth;

        foreach (var item in StartingItems)
            AddItem(item);
    }

    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;
        OnHit?.Invoke();
    }

    public void Heal(int _amount)
    {
        currentHealth += _amount;
        OnHeal?.Invoke();
    }

    public void Kill()
    {
        currentHealth = 0;
        OnDead?.Invoke();
    }

    public void HasShoot()
    {
        foreach (var item in items)
        {
            foreach (var effect in item.shootEffects)
                effect.Execute(this);
        }

        OnShoot?.Invoke();
    }

    public void AddItem(Item _item)
    {
        items.Add(_item);
        foreach (var item in _item.addedEffect)
            item.Execute(this);
    }

    public void RemoveItem(Item _item)
    {
        items.Remove(_item);
    }

    public void RemoveAllIItem()
    {
        items.Clear();
    }

    public bool HasItem(Item _item)
    {
        return items.Contains(_item);
    }

    public void AddStats(Stats _stats)
    {
        currentStats += _stats;
        if (_stats.maxHealth > 0)
            currentHealth += _stats.maxHealth;
    }

    public void RemoveStats(Stats _stats)
    {
        currentStats += _stats;
    }
}
