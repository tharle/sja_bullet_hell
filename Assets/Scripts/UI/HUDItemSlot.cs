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
    }
}
