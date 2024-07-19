using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyDeadState : AEnemyState
{
    public override void Update()
    {
        base.Update();
        m_Owner.CheckPlayerInTauntRange();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER DIE");
        m_Owner.Stop();
        m_Owner.EnemyAnimation.Dead();
        m_Owner.StartCoroutine(WaitToDead());
    }

    private IEnumerator WaitToDead()
    {
        yield return new WaitForSeconds(GameParameters.Prefs.ENEMY_DIE_DURATION);
        m_Owner.DestroyIt();
    }
}
