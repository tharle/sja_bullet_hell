using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDMenuGame : MonoBehaviour
{
    [SerializeField] GameObject m_Menu;
    [SerializeField] GameObject m_MenuStats;
    [SerializeField] GameObject m_MenuItens;
    bool m_MenuOpen;



    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI m_HP;
    [SerializeField] private TextMeshProUGUI m_BulletDmg;
    [SerializeField] private TextMeshProUGUI m_AttackSpd;
    [SerializeField] private TextMeshProUGUI m_BulletSpd;
    [SerializeField] private TextMeshProUGUI m_Range;
    [SerializeField] private TextMeshProUGUI m_Speed;

    void Start()
    {
        m_MenuOpen = false;
        m_Menu.SetActive(m_MenuOpen);

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.MenuOpen, OnMenuOpen);
    }

    private void OnMenuOpen(GameEventMessage message)
    {
        ToogleMenu();

        if (!m_MenuOpen) return;

        if (message.Contains<Stats>(EGameEventMessage.Stats, out Stats stats) 
            && message.Contains<int>(EGameEventMessage.CurrentHealth, out int currentHP))
        {

            LoadStats(stats, currentHP);
        }
         

        if (message.Contains<List<Item>>(EGameEventMessage.Itens, out List<Item> itens)) LoadItens(itens);
    }

    public void ToogleMenu()
    {
        m_MenuOpen = !m_MenuOpen;
        m_Menu.SetActive(m_MenuOpen);

        if(m_MenuOpen) Time.timeScale = 0f;
        else Time.timeScale = 1.0f;
    }

    public void Load()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.ReloadCurrentGame();
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(GameParameters.SceneName.MAIN_MENU);
    }


    private void LoadStats(Stats stats, int currentHP)
    {
        m_HP.text = $"{currentHP}/{stats.MaxHealth}";
        m_BulletDmg.text = stats.Damage.ToString();
        m_AttackSpd.text = $"{stats.BulletPerSeconds} Bullet/s";
        m_BulletSpd.text = $"{stats.BulletSpeed} m/s";
        m_Range.text = $"{stats.BulletRange}m";
        m_Speed.text = $"{stats.Speed}m/s";
    }


    private void LoadItens(List<Item> itens)
    {
        
    }
}
