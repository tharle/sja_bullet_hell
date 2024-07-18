using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLoader 
{
    private Dictionary<EEnemy, EnemyEntity> m_EnemyMap;

    private static EnemyLoader m_Instance;
    public static EnemyLoader Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new();
            }

            return m_Instance;
        }
    }

    EnemyLoader() 
    { 
        LoadAll();
    }

    private void LoadAll()
    {
        m_EnemyMap = new Dictionary<EEnemy, EnemyEntity>();

        List<GameObject> enemies = BundleLoader.Instance.LoadAll<GameObject, EEnemy>(GameParameters.BundleNames.PREFAB_ENEMY);
        foreach (GameObject enemyObject in enemies)
        {
            // TODO fixer ici
            if (enemyObject.TryGetComponent<EnemyEntity>(out EnemyEntity enemy)) m_EnemyMap.Add(enemy.Type, enemy);
        }
    }

    public EnemyEntity Get(EEnemy type)
    {
        if (!m_EnemyMap.ContainsKey(type))
        {
            GameObject enemyObject = BundleLoader.Instance.Load<GameObject>(GameParameters.BundleNames.PREFAB_ENEMY, nameof(type));

            if (enemyObject == null) return null;

            if (enemyObject.TryGetComponent<EnemyEntity>(out EnemyEntity enemy)) m_EnemyMap.Add(enemy.Type, enemy);
            else return null;
        }

        return m_EnemyMap[type];
    }
}
