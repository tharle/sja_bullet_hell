using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[System.Serializable]
public class EnemyAttackState : AEnemyState
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

    protected virtual IEnumerator AttackRoutine()
    {
        while (m_Owner.IsInAttackRange())
        {
            m_Owner.Shoot();

            yield return new WaitForSeconds(m_Owner.Enemy.CooldownAttack);
        }

        m_Owner.ChangeState<EnemyChaseState>();
    }
}
