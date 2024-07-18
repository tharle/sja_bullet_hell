using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Shoot()
    {
        
    }

    public void Hit(float obj)
    {
        
    }

    public void Dead()
    {
        m_Animator.SetTrigger(GameParameters.AnimationEnemy.TRIGGER_DIE);
    }

    // Update is called once per frame
    public void ChangeVelocity(float velocity)
    {
        m_Animator.SetFloat(GameParameters.AnimationEnemy.FLOAT_VELOCITY, velocity);
    }
}
