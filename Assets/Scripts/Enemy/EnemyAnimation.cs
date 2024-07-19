using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator m_Animator;

    private Animator GetAnimator()
    {
        if(m_Animator == null) m_Animator = GetComponent<Animator>();
        return m_Animator;
    }

    public void Shoot()
    {
        
    }

    public void Hit(float obj)
    {
        
    }

    public void Dead()
    {
        GetAnimator().SetTrigger(GameParameters.AnimationEnemy.TRIGGER_DIE);
    }

    // Update is called once per frame
    public void ChangeVelocity(float velocity)
    {
        GetAnimator().SetFloat(GameParameters.AnimationEnemy.FLOAT_VELOCITY, velocity);
    }
}
