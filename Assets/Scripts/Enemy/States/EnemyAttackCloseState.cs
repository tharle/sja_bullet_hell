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
            m_Owner.MoveToPlayer(4);
            AudioManager.Instance.Play(EAudio.SFXBull);
            yield return new WaitForSeconds(m_Owner.Enemy.Fatigue);
        }

        m_Owner.ChangeState<EnemyChaseState>();
    }
}
