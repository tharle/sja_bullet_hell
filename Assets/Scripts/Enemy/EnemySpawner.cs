using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner m_Instance;
    public static EnemySpawner Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("Enemy Loader");
                go.AddComponent<EnemySpawner>();
            }

            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }

    public EnemyEntity SpawnEnemy(EEnemy type, Vector2 position)
    {
        EnemyEntity enemy = EnemyLoader.Instance.Get(type);
        enemy.transform.parent = transform;
        enemy.transform.position = position;
        return Instantiate(enemy);
    }
}
