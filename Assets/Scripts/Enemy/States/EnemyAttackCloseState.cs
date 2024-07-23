using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttackCloseState : EnemyAttackState
{
    protected override IEnumerator AttackRoutine()
    {        
        while (m_Owner.IsInAttackRange())
        {
            m_Owner.MoveToPlayer(5);

            yield return new WaitForSeconds(m_Owner.Enemy.CooldownAttack);
        }

        m_Owner.ChangeState<EnemyChaseState>();
    }
}
