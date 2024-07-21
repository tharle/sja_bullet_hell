using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDItemSlot : MonoBehaviour
{
    [SerializeField] Image m_Icon;
    [SerializeField] TextMeshProUGUI m_NameItem;
    [SerializeField] TextMeshProUGUI m_Description;
    [SerializeField] TextMeshProUGUI m_DescriptionValue;
    [SerializeField] TextMeshProUGUI m_Amount;


    public void Init(Item item)
    {
        m_Icon.sprite = item.Icon;
        m_NameItem.text = item.name;
        UpdateAmount(1);

        // Dans ma logic, les itens de player ont UN EFFECT pour item
        ItemEffect itemEffect = item.GetAllEffects()[0];
        m_Description.text = itemEffect.Name;
        m_DescriptionValue.text = itemEffect.Description;
    }

    public void UpdateAmount(int amount)
    {
        m_Amount.text = $"x{amount.ToString("00")}";
    }
}
