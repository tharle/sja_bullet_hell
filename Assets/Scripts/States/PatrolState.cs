using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolState : State
{
    [SerializeField] private Vector2 m_PatrolTimeRange;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER PATROL");
        Owner.StartCoroutine(WalkRoutine());
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT PATROL");
    }

    public override void Update()
    {
        base.Update();
        Owner.CheckTargetRange();
    }

    private Vector2 GetRandomDirection()
    {
        float x = Random.Range(Vector2.left.x, Vector2.right.x);
        float y = Random.Range(Vector2.up.y, Vector2.down.y);
        Vector2 direction = new Vector2(x, y);
        direction.Normalize();
        return direction;
    }

    private float GetWalkTime()
    {
        return Random.Range(m_PatrolTimeRange.x, m_PatrolTimeRange.y);
    }

    private IEnumerator WalkRoutine()
    {
        Vector2 direction = GetRandomDirection();
        
        if(direction.x == 0 && direction.y == 0)
        {
            Owner.ChangeState<IdleState>();
            yield break;
        }

        Owner.Move(direction);
        yield return new WaitForSeconds(GetWalkTime());
        Owner.ChangeState<IdleState>();
    }


}
