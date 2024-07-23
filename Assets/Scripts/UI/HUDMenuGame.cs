using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ItemSlotData
{
    public int Amount;
    public Item Item;
    public HUDItemSlot Slot;
}

public class HUDMenuGame : MonoBehaviour
{

    [Header("Stats")]
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

    [Header("Itens")]
    [SerializeField] private GameObject m_ItensContent;
    [SerializeField] private HUDItemSlot m_ItemSlotPrefab;
    Dictionary<EItem, ItemSlotData> m_ItensSlot = new();

    [Header("Game Over")]
    [SerializeField] private GameObject m_GameOver;
    [SerializeField] private TextMeshProUGUI m_WaveIndex;

    void Start()
    {
        m_MenuOpen = false;
        m_Menu.SetActive(m_MenuOpen);
        m_GameOver.SetActive(false);

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.MenuOpen, OnMenuOpen);
        GameEventSystem.Instance.SubscribeTo(EGameEvent.GameOver, OnGameOver);
    }

    private void OnDestroy()
    {
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.MenuOpen, OnMenuOpen);
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.GameOver, OnGameOver);
    }

    private void OnMenuOpen(GameEventMessage message)
    {
        ToogleMenu();
        ChangeToSubMenuStats();

        if (!m_MenuOpen) return;

        if (message.Contains<Stats>(EGameEventMessage.Stats, out Stats stats) 
            && message.Contains<int>(EGameEventMessage.CurrentHealth, out int currentHP))
        {

            LoadStats(stats, currentHP);
        }
         

        if (message.Contains<List<Item>>(EGameEventMessage.Itens, out List<Item> itens)) LoadItens(itens);
    }


    private void OnGameOver(GameEventMessage message)
    {
        if(message.Contains<int>(EGameEventMessage.WaveIndex, out int WaveIndex))
        {
            m_WaveIndex.text = WaveIndex.ToString("00");
            m_GameOver.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    public void ToogleMenu()
    {
        m_MenuOpen = !m_MenuOpen;
        m_Menu.SetActive(m_MenuOpen);

        if(m_MenuOpen) Time.timeScale = 0f;
        else Time.timeScale = 1.0f;
    }

    public void ChangeToSubMenuItens()
    {
        m_MenuStats.SetActive(false);
        m_MenuItens.SetActive(true);
    }

    public void ChangeToSubMenuStats()
    {
        m_MenuStats.SetActive(true);
        m_MenuItens.SetActive(false);
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
        // Clean Amounts
        List<EItem> keys = new List<EItem>(m_ItensSlot.Keys);
        foreach (EItem itemType in keys) 
        {
            ItemSlotData itemSlot = m_ItensSlot[itemType];
            itemSlot.Amount = 0;
            m_ItensSlot[itemType] = itemSlot;
        }


        // Create all itens in invetentory
        foreach(Item item in itens) 
        {
            ItemSlotData itemSlot;

            if (m_ItensSlot.ContainsKey(item.Type))
            {
                itemSlot = m_ItensSlot[item.Type];
                itemSlot.Amount += 1;
                itemSlot.Slot.UpdateAmount(itemSlot.Amount);
                m_ItensSlot[item.Type] = itemSlot;
                continue;
            }

            itemSlot.Item = item;
            itemSlot.Amount = 1;
            itemSlot.Slot = Instantiate(m_ItemSlotPrefab);
            itemSlot.Slot.transform.SetParent(m_ItensContent.transform);
            itemSlot.Slot.Init(item);
            m_ItensSlot.Add(item.Type, itemSlot);
        }
    }
}
