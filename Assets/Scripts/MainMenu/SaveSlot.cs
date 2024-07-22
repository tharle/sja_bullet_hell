using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct SaveSlotData
{
    public string Info;
    public Sprite Icon;
}

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private int m_SlotIndex;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_Info;
    [SerializeField] private TextMeshProUGUI m_ButtonText;

    private SaveSlotData m_DefaultSlotData;
    private bool m_HasData;
    private bool m_IsNewGame;

    private void Start()
    {
        m_DefaultSlotData.Info = m_Info.text;
        m_DefaultSlotData.Icon = m_Icon.sprite;
        m_HasData = false;

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.MainMenuLoadSlot, OnLoadSlot);
    }


    private void OnDestroy()
    {
        GameEventSystem.Instance.UnsubscribeFrom(EGameEvent.MainMenuLoadSlot, OnLoadSlot);
    }

    private void OnLoadSlot(GameEventMessage message)
    {
        if (!message.Contains<int>(EGameEventMessage.SlotIndex, out int SlotIndex) || SlotIndex != m_SlotIndex)
        {
            return;
        }

        message.Contains<bool>(EGameEventMessage.IsNewGame, out m_IsNewGame);

        if (message.Contains<SaveSlotData>(EGameEventMessage.SlotData, out SaveSlotData slotData))
        {
            m_Info.text = slotData.Info;
            m_Icon.sprite = slotData.Icon;
            m_HasData = true;
            m_ButtonText.text = m_IsNewGame ? "Overwrite" : "Load";
        } else
        {
            EmptySlot();
        }

    }

    public void EmptySlot()
    {
        m_Info.text = m_DefaultSlotData.Info;
        m_Icon.sprite = m_DefaultSlotData.Icon;
        m_ButtonText.text = "New";
        m_HasData = false;
    }

    public void OnClickSlot()
    {
        AudioManager.Instance.Play(EAudio.SFXConfirm, transform.position);

        GameEventMessage message = new GameEventMessage(EGameEventMessage.SlotIndex, m_SlotIndex);
        message.Add(EGameEventMessage.HasData, m_HasData && !m_IsNewGame);

        GameEventSystem.Instance.TriggerEvent(EGameEvent.MainMenuSelectSlot, message);
    }

}
