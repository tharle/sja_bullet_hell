using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDWaveInfos : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_WaveInfo;
    [SerializeField] TextMeshProUGUI m_EnnemisCount;



    private void Start()
    {
        SubscribeAll();
    }

    private void SubscribeAll()
    {
        GameEventSystem.Instance.SubscribeTo(EGameEvent.WaveInfoChanged, OnWaveEnter);
    }

    private void OnWaveEnter(GameEventMessage message)
    {
        if(message.Contains<WaveData>(EGameEventMessage.WaveData, out WaveData wave))
        {
            m_WaveInfo.text = $"Wave {wave.Index}";
            m_EnnemisCount.text = $"{wave.EnnemiesDeads.ToString("00")} / {wave.EnnemiesAmount.ToString("00")}";
        }
    }
}
