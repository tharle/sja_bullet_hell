using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameWaveState : AGameState
{
    [SerializeField] private int m_EnemyPerWaveBaseAmount;
    private bool m_EndSpaw;
    private int m_EnemiesCount;


    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER Game WaveState ");
        Debug.Log($"START Wave {m_Owner.WaveCount} ");

        m_EndSpaw = false;
        m_EnemiesCount = 0;
        m_Owner.StartCoroutine(SpawnEnemies());
    }
    public override void Update()
    {
        base.Update();

        if (!m_EndSpaw) return;

        if (AllEnemiesAreDead()) m_Owner.ChangeState<GameWaveState>();
    }

    private bool AllEnemiesAreDead()
    {
        return m_EnemiesCount <= 0;
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT Game WaveState");
    }

    private IEnumerator SpawnEnemies()
    {
        m_Owner.WaveCount++;

        for (int i = 0; i < m_EnemyPerWaveBaseAmount; i++)
        {
            for (int j = 0; j< m_Owner.WaveCount; j++)
            {
                Vector2 position = GetRandomSpotPosition();
                EnemyEntity enemy = EnemySpawner.Instance.SpawnEnemy(EEnemy.Cow, position);
                EffectManager.Instance.CastEffect(EEffect.Summon, position, enemy.ColorShow, 0);

                yield return new WaitForSeconds(0.5f);
                enemy = m_Owner.Cast<EnemyEntity>(enemy);
                enemy.OnDead += OnEnemyDead;
                m_EnemiesCount++;
                yield return new WaitForSeconds(GameParameters.Prefs.ENEMY_SPAWN_COOLDOWN);

            }

            yield return new WaitForSeconds(GameParameters.Prefs.WAVE_COOLDOWN);
        }


        m_EndSpaw = true;
    }
    private void OnEnemyDead()
    {
        m_EnemiesCount--;
    }

    public Vector2 GetRandomSpotPosition()
    {
        if (m_Owner.Spots.Count <= 0) return Vector2.zero;

        int index = Random.Range(0, m_Owner.Spots.Count);
        return m_Owner.Spots[index].position;
    }

}
