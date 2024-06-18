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
        m_CurrentHealth = _save.CurrentHealth;
        m_Items = _save.Items;
        m_CurrentStats = _save.Stats;
    }
}
