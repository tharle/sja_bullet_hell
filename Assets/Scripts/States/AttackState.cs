using System.Collections;
using UnityEngine;


[System.Serializable]
public class AttackState : State
{
    [SerializeField] float m_DelayAttack;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ENTER ATTACK");
        Owner.Stop();
        Owner.StartCoroutine(AttackRoutine());
    }

    public override void Update()
    {
        base.Update();        
    }

    public override void OnExit()
    {
        base.OnEnter();
        Debug.Log("EXIT ATTACK");
    }

    private IEnumerator AttackRoutine()
    {
        while (Owner.IsInAttackRange())
        {
            // TODO do attack
            Debug.Log("HE ATACKS :3333");
            yield return new WaitForSeconds(m_DelayAttack);
        }

        Owner.ChangeState<ChaseState>();
    }
}
