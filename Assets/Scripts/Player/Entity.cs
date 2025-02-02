using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable, IContainer
{
    protected const float MULTIPLI_PIXEL = GameParameters.Prefs.GRID_SIZE_IN_PIXEL;

    public Action OnDead;
    public Action<float> OnHit;
    public Action<float> OnHeal;
    public Action OnShoot;

    [Header("Entity")]
    [SerializeField] private EntityData m_Data;

    protected List<Item> m_Items = new List<Item>();
    protected Stats m_CurrentStats;
    protected int m_CurrentHealth;
    protected EBullet m_CurrentBulletType;
    public EBullet BulletType => m_CurrentBulletType;

    public GameObject BulletPrefab => m_Data.ProjectilePrefab;
    public Stats Stats => m_CurrentStats;
    public Color EntityColor => m_Data.Color;
    public int CurrentHealth => m_CurrentHealth;
    public List<Item> StartingItems => m_Data.StartingItems;
    public List<Item> Items { get => m_Items; set { } }

    private bool m_DeadWasNotify = false;

    protected virtual void Awake()
    {
        m_CurrentStats = m_Data.stats;
        m_CurrentHealth = m_CurrentStats.MaxHealth;
        m_CurrentBulletType = m_Data.Bullet;

        foreach (var item in StartingItems)
            AddItem(item);
    }

    public void TakeDamage(int amount)
    {
        m_CurrentHealth -= amount;
        OnHit?.Invoke( (float)m_CurrentHealth/ (float)m_CurrentStats.MaxHealth);

        if (!IsAlive()) Kill();
    }

    public void Heal(int amount)
    {
        m_CurrentHealth += amount;
        OnHeal?.Invoke((float)m_CurrentHealth / (float)m_CurrentStats.MaxHealth);
    }

    virtual public void Kill()
    {
        if (m_DeadWasNotify) return;

        m_DeadWasNotify = true;
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

    public void AddItem(Item item)
    {
        m_Items.Add(item);
        foreach (var effect in item.addedEffect)
            effect.Execute(this);

        foreach(var effect in item.pickupEffect)
            effect.Execute(this);
    }

    public void RemoveItem(Item item)
    {
        m_Items.Remove(item);
    }

    public void RemoveAllIItem()
    {
        m_Items.Clear();
    }

    public bool HasItem(Item item)
    {
        return m_Items.Contains(item);
    }

    public void AddStats(Stats stats)
    {
        m_CurrentStats += stats;
        if (stats.MaxHealth > 0)
            m_CurrentHealth += stats.MaxHealth;
    }

    public void RemoveStats(Stats stats)
    {
        m_CurrentStats += stats;
    }

    public bool IsAlive()
    {
        return m_CurrentHealth > 0;
    }
}
