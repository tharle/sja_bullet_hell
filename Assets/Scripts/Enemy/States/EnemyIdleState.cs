using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyIdleState : AEnemyState
{
    public override void Update()
    {
        base.Update();
        m_Owner.CheckPlayerInTauntRange();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER IDLE");
        m_Owner.StartCoroutine(WaitRoutine());
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT IDLE");
    }

    private IEnumerator WaitRoutine()
    {
        m_Owner.Stop();
        yield return new WaitForSeconds(GetWaitTime());
        m_Owner.ChangeState<EnemyPatrolState>();
    }

    private float GetWaitTime()
    {
        return m_Owner.Enemy.IdleTime;
    }
}
