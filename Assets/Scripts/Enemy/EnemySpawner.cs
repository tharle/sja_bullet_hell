using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<EnemyEntity> m_Enemies = new();

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
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }

    public EnemyEntity SpawnEnemy(EEnemy type, Vector2 position)
    {
        EnemyEntity enemy = m_Enemies.Find(enemy => enemy.isActiveAndEnabled && enemy.Type == type);

        if (enemy == null) enemy = SpawnNewEnemy(type, position);
        else enemy.transform.position = position;

        return Instantiate(enemy);
    }

    

    private EnemyEntity SpawnNewEnemy(EEnemy type, Vector2 position)
    {
        EnemyEntity enemy = EnemyLoader.Instance.Get(type);
        enemy.transform.parent = transform;
        m_Enemies.Add(enemy);
        return EnemyLoader.Instance.Get(type);
    }

}
