using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Save
{
    [SerializeField] private bool newGame;
    [SerializeField] private int index;
    [SerializeField] private string saveName;
    [SerializeField] private List<Item> items;
    [SerializeField] int currentHealth;
    [SerializeField] private Stats stats;

    public string SaveName => saveName;
    public List<Item> Items => items;
    public Stats Stats => stats;
    public int CurrentHealth => currentHealth;
    public int Index => index;
    public bool NewGame { get => newGame; set => newGame = value; }

    public Save()
    {
        index = 0;
        saveName = "Save 0";
        items = new List<Item>();
        currentHealth = 0;
    }

    public Save(int _index,string _saveName,List<Item> _items,int _currentHealth)
    { 
        index = _index;
        saveName = _saveName;
        items = _items;
        currentHealth = _currentHealth;
    }

    public Save(bool _newGame,int _index, string _saveName, List<Item> _items, int _currentHealth)
    {
        newGame = _newGame;
        index = _index;
        saveName = _saveName;
        items = _items;
        currentHealth = _currentHealth;
    }

    public void UpdateSave(List<Item> _items, int _currentHealth,Stats _stats)
    {
        stats = _stats; 
        items = _items;
        currentHealth = _currentHealth;
    }
}
