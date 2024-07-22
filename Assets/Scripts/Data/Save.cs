using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    [SerializeField] private bool m_IsNewGame;
    [SerializeField] private int m_Index;
    [SerializeField] private string m_SaveName;
    [SerializeField] private List<Item> m_Items;
    [SerializeField] int m_CurrentHealth;
    [SerializeField] private Stats m_Stats;
    [SerializeField] private int m_WaveIndex;

    public string SaveName => m_SaveName;
    public List<Item> Items => m_Items;
    public Stats Stats => m_Stats;
    public int CurrentHealth => m_CurrentHealth;
    public int Index => m_Index;
    public bool IsNewGame { get => m_IsNewGame; set => m_IsNewGame = value; }
    public int WaveIndex => m_WaveIndex;

    public Save()
    {
        m_Index = 0;
        m_SaveName = "Save 0";
        m_Items = new List<Item>();
        m_CurrentHealth = 0;
    }

    public Save(bool _newGame,int _index, string _saveName, List<Item> _items, int _currentHealth, int waveIndex)
    {
        m_IsNewGame = _newGame;
        m_Index = _index;
        m_SaveName = _saveName;
        m_Items = _items;
        m_CurrentHealth = _currentHealth;
        m_WaveIndex = waveIndex;
    }

    public void UpdateSave(List<Item> _items, int _currentHealth,Stats _stats, int waveIndex)
    {
        m_Stats = _stats; 
        m_Items = _items;
        m_CurrentHealth = _currentHealth;
        m_WaveIndex = waveIndex;
    }
}
