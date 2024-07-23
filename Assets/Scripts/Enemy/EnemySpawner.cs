using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void OnDestroy()
    {
        EnemyLoader.Instance.Destroy();
        m_Instance = null;
    }

    public EnemyEntity SpawnEnemy(EEnemy type, Vector2 position)
    {
        EnemyEntity enemy = EnemyLoader.Instance.Get(type);
        enemy.transform.parent = transform;
        enemy.transform.position = position;
        return enemy;
    }

    public EnemyEntity SpawnRandom(int WaveIndex, Vector2 position)
    {
        // TODO: use Wave index for calculs of spawn ennemies

        List<EEnemy> typesEnemies = System.Enum.GetValues(typeof(EEnemy)).Cast<EEnemy>().ToList();

        int randomId = UnityEngine.Random.Range(0, typesEnemies.Count);

        return SpawnEnemy(typesEnemies[randomId], position);
    }
}
