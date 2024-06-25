using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColletable : MonoBehaviour
{
    [SerializeField] private EItem m_ItemType;
    [SerializeField] private SpriteRenderer m_SpriteModel;
    private Item m_Item;

    private void Start()
    {
        LoadItem();
    }

    private void LoadItem()
    {
        m_Item = ItemLoader.Instance.Get(m_ItemType);

        if (m_Item.Type == EItem.None) Destroy(gameObject);

        m_SpriteModel.sprite = m_Item.Icon;
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
