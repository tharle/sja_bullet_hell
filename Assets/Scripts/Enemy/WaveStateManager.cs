using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStateManager : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Spots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            EnemySpawner.Instance.SpawnEnemy(EEnemy.COW, GetRandomSpotPosition());
    }


    private Vector2 GetRandomSpotPosition()
    {
        if (m_Spots.Count <= 0) return Vector2.zero;

        int index = Random.Range(0, m_Spots.Count);
        return m_Spots[index].position;
    }
}
