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
    [SerializeField] private EntityData m_Data;
    protected List<Item> m_Items = new List<Item>();
    protected Stats m_CurrentStats;
    protected int m_CurrentHealth;

    public GameObject BulletPrefab => m_Data.ProjectilePrefab;
    public Stats Stats => m_CurrentStats;
    public Color EntityColor => m_Data.Color;
    public int CurrentHealth => m_CurrentHealth;
    public List<Item> StartingItems => m_Data.StartingItems;
    public List<Item> Items { get => m_Items; set { } }

    protected virtual void Awake()
    {
        m_CurrentStats = m_Data.stats;
        m_CurrentHealth = m_CurrentStats.MaxHealth;

        foreach (var item in StartingItems)
            AddItem(item);
    }

    public void TakeDamage(int _amount)
    {
        m_CurrentHealth -= _amount;
        OnHit?.Invoke();
    }

    public void Heal(int _amount)
    {
        m_CurrentHealth += _amount;
        OnHeal?.Invoke();
    }

    public void Kill()
    {
        m_CurrentHealth = 0;
        OnDead?.Invoke();
    }

    public void HasShoot()
    {
        foreach (var item in m_Items)
        {
            foreach (var effect in item.shootEffects)
                effect.Execute(this);
        }

        OnShoot?.Invoke();
    }

    public void AddItem(Item _item)
    {
        m_Items.Add(_item);
        foreach (var item in _item.addedEffect)
            item.Execute(this);
    }

    public void RemoveItem(Item _item)
    {
        m_Items.Remove(_item);
    }

    public void RemoveAllIItem()
    {
        m_Items.Clear();
    }

    public bool HasItem(Item _item)
    {
        return m_Items.Contains(_item);
    }

    public void AddStats(Stats _stats)
    {
        m_CurrentStats += _stats;
        if (_stats.MaxHealth > 0)
            m_CurrentHealth += _stats.MaxHealth;
    }

    public void RemoveStats(Stats _stats)
    {
        m_CurrentStats += _stats;
    }
}
