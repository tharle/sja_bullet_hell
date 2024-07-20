using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyDeadState : AEnemyState
{
    [SerializeField] float m_Duration = GameParameters.Prefs.ENEMY_DIE_DURATION;

    public override void Update()
    {
        base.Update();
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
        m_Owner.DestroyIt(m_Duration);
        yield return new WaitForSeconds(m_Duration);
        
    }
}
