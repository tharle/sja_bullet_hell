using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolState : State
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER PATROL");
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT PATROL");
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            Owner.ChangeState<IdleState>();
    }

    
}
