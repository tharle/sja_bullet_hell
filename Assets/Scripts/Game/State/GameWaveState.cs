using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameWaveState : AGameState
{
    [SerializeField] private int m_EnemyPerWaveBaseAmount;
    private bool m_EndSpaw;


    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER Game WaveState ");

        m_EndSpaw = false;
        m_Owner.StartCoroutine(SpawnEnemies());

        if(IsElliteTime()) m_Owner.StartCoroutine(SpawnElite());
    }

    public override void Update()
    {
        base.Update();

        if (!m_EndSpaw) return;

        if (m_Owner.Wave.AllEnemiesAreDead()) m_Owner.ChangeState<GameWaveState>();
    }

    

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT Game WaveState");
    }

    private void NotifyWaveInfos() 
    {
        GameEventSystem.Instance.TriggerEvent(EGameEvent.WaveInfoChanged, new GameEventMessage(EGameEventMessage.WaveData, m_Owner.Wave));
    }

    private IEnumerator SpawnEnemies()
    {

        m_Owner.Wave.Index++;
        m_Owner.Wave.EnnemiesAmount = (m_EnemyPerWaveBaseAmount * m_Owner.Wave.Index);
        if (IsElliteTime()) m_Owner.Wave.EnnemiesAmount++;
        m_Owner.Wave.EnnemiesDeads = 0;
        yield return null;
        NotifyWaveInfos();

        for (int i = 0; i < m_EnemyPerWaveBaseAmount; i++)
        {
            for (int j = 0; j< m_Owner.Wave.Index; j++)
            {
                Vector2 position = GetRandomSpotPosition();
                EnemyEntity enemy = EnemySpawner.Instance.SpawnRandom(position);
                EffectManager.Instance.CastEffect(EEffect.Summon, position, enemy.ColorShow, 0);

                yield return new WaitForSeconds(0.5f);
                enemy = m_Owner.Cast<EnemyEntity>(enemy);
                enemy.OnDead += OnEnemyDead;
                yield return new WaitForSeconds(GameParameters.Prefs.ENEMY_SPAWN_COOLDOWN);

            }

            yield return new WaitForSeconds(GameParameters.Prefs.WAVE_COOLDOWN);
        }


        m_EndSpaw = true;
    }

    private IEnumerator SpawnElite()
    {
        int index = m_Owner.Wave.Index;
        float min = GameParameters.Prefs.WAVE_COOLDOWN * index/3;
        float max = min * (index - 1) ;
        yield return new WaitForSeconds(UnityEngine.Random.Range(min, max));

        Vector2 position = GetRandomSpotPosition();
        EnemyEntity enemy = EnemySpawner.Instance.SpawnRandom(position, index);
        EffectManager.Instance.CastEffect(EEffect.Summon, position, enemy.ColorShow, 0);

        yield return new WaitForSeconds(0.5f);
        enemy = m_Owner.Cast<EnemyEntity>(enemy);
        enemy.SetElite();
        enemy.OnDead += OnEnemyDead;
        yield return new WaitForSeconds(GameParameters.Prefs.ENEMY_SPAWN_COOLDOWN);


    }

    private bool IsElliteTime()
    {
        return m_Owner.Wave.Index % 3 == 0;
    }
    private void OnEnemyDead()
    {
        m_Owner.Wave.EnnemiesDeads++;
        NotifyWaveInfos();
    }

    public Vector2 GetRandomSpotPosition()
    {
        if (m_Owner.Spots.Count <= 0) return Vector2.zero;

        int index = UnityEngine.Random.Range(0, m_Owner.Spots.Count);
        return m_Owner.Spots[index].position;
    }

}
