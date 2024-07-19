using System.Collections;
using UnityEngine;


[System.Serializable]
public class EnemyChaseState : AEnemyState
{
    private float m_ElapseTime;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER CHASE");

        m_Owner.StopAllCoroutines();
        m_Owner.StartCoroutine(MoveToPlayerRoutine());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT CHASE");
    }

    private IEnumerator MoveToPlayerRoutine()
    {
        
        m_ElapseTime = 0.0f;
        while (m_ElapseTime < m_Owner.Enemy.Fatigue)
        {
            m_Owner.MoveToPlayer();
            yield return new WaitForSeconds(GameParameters.Prefs.ENEMY_TICK_CHECK_IN_SECONDS);
            m_ElapseTime += GameParameters.Prefs.ENEMY_TICK_CHECK_IN_SECONDS;

            if (m_Owner.IsInAttackRange())
            {
                m_Owner.ChangeState<EnemyAttackState>();
                yield break;
            }
        }

        m_Owner.ChangeState<EnemyIdleState>();
    }
}
