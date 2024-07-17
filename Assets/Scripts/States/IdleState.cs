using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : State
{
    [SerializeField] private Vector2 m_WaitTimeRange;

    public override void Update()
    {
        base.Update();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER IDLE");
        Owner.StartCoroutine(WaitRoutine());
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT IDLE");
    }

    private IEnumerator WaitRoutine()
    {
        Owner.Stop();
        yield return new WaitForSeconds(GetWaitTime());
        Owner.ChangeState<PatrolState>();
    }

    private float GetWaitTime()
    {
        return Random.Range(m_WaitTimeRange.x, m_WaitTimeRange.y);
    }
}
