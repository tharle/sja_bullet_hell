using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPatrolState : AEnemyState
{

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER PATROL");
        m_Owner.StartCoroutine(WalkRoutine());
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT PATROL");
    }

    public override void Update()
    {
        base.Update();
        m_Owner.CheckPlayerInTauntRange();
    }

    private Vector2 GetRandomDirection()
    {
        float x = Random.Range(Vector2.left.x, Vector2.right.x);
        float y = Random.Range(Vector2.up.y, Vector2.down.y);
        Vector2 direction = new Vector2(x, y);
        direction.Normalize();
        return direction;
    }

    private IEnumerator WalkRoutine()
    {
        Vector2 direction = GetRandomDirection();

        if (direction.x == 0 && direction.y == 0)
        {
            m_Owner.ChangeState<EnemyIdleState>();
            yield break;
        }
        m_Owner.Move(direction);
        yield return new WaitForSeconds(m_Owner.Enemy.PatrolTime);
        m_Owner.ChangeState<EnemyIdleState>();
    }


}
