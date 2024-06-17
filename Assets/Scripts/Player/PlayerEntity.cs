using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    protected override void Awake()
    {
        base.Awake();

        if (GameManager.Instance != null)
            InitPlayer();
        else
            GameManager.OnInit += InitPlayer;
    }

    public void InitPlayer()
    {
        GameManager.Instance.SetPlayer(this);

        GameManager.OnInit -= InitPlayer;
    }

    public void SetSave(Save _save)
    {
        currentHealth = _save.CurrentHealth;
        items = _save.Items;
        currentStats = _save.Stats;
    }
}
