using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolState : State
{
    [SerializeField] private List<Transform> m_Spots;
    [SerializeField] private float m_WaitTime;
    [SerializeField] private float m_MinDistance;

    private int m_CurrentSpot;
    private bool m_Walking;
    private Vector2 m_Direction;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER PATROL");
        m_Walking = false;
        m_CurrentSpot = 0;

        if (m_Spots.Count <= 0) Owner.ChangeState<IdleState>();
        Owner.StartCoroutine(WaitRoutine());
    }

    public override void OnExit()
    {
        base.OnEnter();
        Owner.StopAllCoroutines();
        Debug.Log("EXIT PATROL");
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            Owner.ChangeState<IdleState>();

        if(m_Walking)
        {
            Owner.Move(m_Direction);

            Transform nextSpot = m_Spots[m_CurrentSpot];
            if (Vector2.Distance(nextSpot.position, Owner.transform.position) <= m_MinDistance)
                Owner.StartCoroutine(WaitRoutine());
        }
    }

    private void DoWalk()
    {
        
        m_CurrentSpot++;
        m_CurrentSpot = m_CurrentSpot % m_Spots.Count;
        Transform nextSpot = m_Spots[m_CurrentSpot];
        m_Direction = Vector3.Normalize(nextSpot.position - Owner.transform.position);
        m_Walking = true;
    }

    private IEnumerator WaitRoutine()
    {
        m_Walking = false;
        Owner.Stop();
        yield return new WaitForSeconds(m_WaitTime);
        DoWalk();
    }


}
