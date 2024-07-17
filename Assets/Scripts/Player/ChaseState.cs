using System.Collections;
using UnityEngine;


[System.Serializable]
public class ChaseState : State
{
    [SerializeField] private float m_FatigueTime = 1.0f;

    private float m_ElapseTime;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER CHASE");

        Owner.StopAllCoroutines();
        Owner.StartCoroutine(MoveToPlayerRoutine());
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
        while (m_ElapseTime < m_FatigueTime)
        {
            Owner.MoveToPlayer();
            yield return new WaitForSeconds(0.1f);
            m_ElapseTime += 0.1f;

            if (Owner.IsInAttackRange())
            {
                Owner.ChangeState<AttackState>();
                yield break;
            }
        }

        Owner.ChangeState<IdleState>();
    }
}
