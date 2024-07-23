using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColletable : MonoBehaviour
{
    [SerializeField] private EItem m_ItemType;
    [SerializeField] private SpriteRenderer m_SpriteModel;
    private Item m_Item;

    public Item Item { get { return m_Item; } 
        set { 
            m_Item = value;
            m_ItemType = value.Type;
        } 
    }

    public void LoadItem(bool withDispaw = false)
    {
        m_Item = ItemLoader.Instance.Get(m_ItemType);

        m_SpriteModel.sprite = m_Item.Icon;

        if (withDispaw) Destroy(gameObject, GameParameters.Prefs.ITEM_DESPAWN_TIME);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameParameters.TagName.PLAYER)) 
        {
            GameEventSystem.Instance.TriggerEvent(EGameEvent.AddItem, new GameEventMessage(EGameEventMessage.Item, m_Item));
            Destroy(gameObject, 0.01f);
        }
    }
}
