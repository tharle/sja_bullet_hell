using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemLoader : MonoBehaviour 
{
    #region Singleton
    private static ItemLoader m_Instance;
    public static ItemLoader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("Item Loader");
                go.AddComponent<ItemLoader>();
            }

            return m_Instance;
        }
    }
    #endregion

    private Dictionary<EItem, Item> m_Items; // Name/ItemScriptableObject


    private void Awake()
    {
        if (m_Instance != null && m_Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
        m_Items = new Dictionary<EItem, Item>();   
    }

    private void Start()
    {
        LoadAll();
        
    }

    public void LoadAll(bool forceLoad = false)
    {
        if (m_Items.Count > 0 && !forceLoad) return;

        List<Item> items = BundleLoader.Instance.LoadAll<Item, EItem>(GameParameters.BundleNames.ITEM, true);
        foreach (Item itemData in items)
        {
            if(itemData == null) continue;

            if (!m_Items.ContainsKey(itemData.Type)) m_Items.Add(itemData.Type, itemData);
            else m_Items[itemData.Type] = itemData;
        }
    }

    public Item Get(EItem itemType)
    {
        if (m_Items.Count <= 0) LoadAll();

        if (!m_Items.ContainsKey(itemType))
        {
            Item itemData = BundleLoader.Instance.Load<Item>(GameParameters.BundleNames.ITEM, nameof(itemType));
            m_Items.Add(itemType, itemData);
        }
        Item item = Instantiate(m_Items[itemType]);
        item.name = m_Items[itemType].name;
        return item;
    }

    public List<Item> GetAll()
    {
        if(m_Items.Count <= 0) LoadAll();

        List<Item> result = new List<Item>();

        foreach (Item itemData in m_Items.Values)
        {
            Item item = Instantiate(itemData);
            item.name = itemData.name;
            result.Add(item);
        }

        return result;
    }
}
