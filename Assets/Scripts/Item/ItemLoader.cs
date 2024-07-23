

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private ItemColletable m_ItemColletablePrefab;


    private void Awake()
    {
        if (m_Instance != null && m_Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
        m_Items = new Dictionary<EItem, Item>();
        LoadItemColletable();
        LoadAlltens();
        
    }

    private void LoadItemColletable()
    {
        GameObject go = BundleLoader.Instance.Load<GameObject>(GameParameters.BundleNames.PREFAB_ITEM_COLLETABLE, GameParameters.BundleNames.PREFAB_ITEM_COLLETABLE);

        if (go.TryGetComponent<ItemColletable>(out m_ItemColletablePrefab))
        {
            m_ItemColletablePrefab.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ItemLoader : Can't load Item colletable.");
        }
    }

    public void LoadAlltens(bool forceLoad = false)
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
        if (m_Items.Count <= 0) LoadAlltens();

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
        if(m_Items.Count <= 0) LoadAlltens();

        List<Item> result = new List<Item>();

        foreach (Item itemData in m_Items.Values)
        {
            Item item = Instantiate(itemData);
            item.name = itemData.name;
            result.Add(item);
        }

        return result;
    }

    private ItemColletable GetRandom()
    {
        List<EItem> typesIten = System.Enum.GetValues(typeof(EItem)).Cast<EItem>().ToList();

        int randomId = Random.Range(0, typesIten.Count);
        EItem randomItemType = typesIten[randomId];

        ItemColletable itemColletable = Instantiate(m_ItemColletablePrefab);
        itemColletable.Item = m_Items[randomItemType];
        return itemColletable;
    }

    public void DropRandomItem(Vector3 position, float delay)
    {
        StartCoroutine(DoDropRandom(position, delay));
    }

    private IEnumerator DoDropRandom(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        ItemColletable itemColletable = GetRandom();
        itemColletable.gameObject.SetActive(true);
        itemColletable.gameObject.transform.position = position;
        itemColletable.LoadItem();
    }
}
