using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DeadState : State
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
    }
}
