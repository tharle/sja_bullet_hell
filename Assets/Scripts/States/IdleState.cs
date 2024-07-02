using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IdleState : State
{

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
            Owner.ChangeState<PatrolState>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER IDLE");
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT IDLE");
    }
}
