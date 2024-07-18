using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[System.Serializable]
public class AttackState : State
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER ATTACK");
        m_Owner.Stop();
        m_Owner.StartCoroutine(AttackRoutine());
    }

    public override void Update()
    {
        base.Update();        
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT ATTACK");
    }

    private IEnumerator AttackRoutine()
    {
        while (m_Owner.IsInAttackRange())
        {
            // TODO do attack
            Debug.Log("HE ATACKS :3333");
            m_Owner.Shoot();

            yield return new WaitForSeconds(m_Owner.Enemy.CooldownAttack);
        }

        m_Owner.ChangeState<ChaseState>();
    }
}
