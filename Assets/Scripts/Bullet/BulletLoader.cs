using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBullet
{
    Default,
    Milk
}

public class BulletLoader : MonoBehaviour
{
    #region Singleton
    private static BulletLoader m_Instance;
    public static BulletLoader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("Bullet Loader");
                go.AddComponent<BulletLoader>();
            }

            return m_Instance;
        }
    }
    #endregion

    private Dictionary<EBullet, GameObject> m_Bullets;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
        m_Bullets = new Dictionary<EBullet, GameObject>();
    }

    private void Start()
    {
        LoadAll();

    }

    public void LoadAll(bool forceLoad = false)
    {
        if (m_Bullets.Count > 0 && !forceLoad) return;

        List<GameObject> bullets = BundleLoader.Instance.LoadAll<GameObject>(GameParameters.BundleNames.BULLET, true);
        foreach (GameObject bulletPrefab in bullets)
        {
            if (Enum.TryParse<EBullet>(bulletPrefab.name, true, out EBullet bulletType))
            {
                if (!m_Bullets.ContainsKey(bulletType)) m_Bullets.Add(bulletType, bulletPrefab);
                else m_Bullets[bulletType] = bulletPrefab;
            }
        }
    }

    public GameObject Get(EBullet bulletType)
    {
        if (m_Bullets.Count <= 0) LoadAll();

        if (!m_Bullets.ContainsKey(bulletType))
        {
            GameObject bulletPrefab = BundleLoader.Instance.Load<GameObject>(GameParameters.BundleNames.BULLET, nameof(bulletType));
            m_Bullets.Add(bulletType, bulletPrefab);
        }
        GameObject bullet = Instantiate(m_Bullets[bulletType]);
        bullet.name = m_Bullets[bulletType].name;
        return bullet;
    }
}
