using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_IsLookRight;
    public bool IsLookRight { get => m_IsLookRight; }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_IsLookRight = true;
    }

    public void AddVelocity(float velocity)
    {
        m_Animator.SetFloat("velocity", velocity);
    }

    public void DoFlip()
    {
        m_IsLookRight = !m_IsLookRight;

        if (m_IsLookRight) m_Animator.SetTrigger("look_to_right");
        else m_Animator.SetTrigger("look_to_left");

        Vector3 scale = m_Animator.transform.localScale;
        scale.x = -scale.x;
        m_Animator.transform.localScale = scale;
    }

    public void OnAnimationFlip()
    {
        /*Vector3 scale = m_Animator.transform.localScale;
        scale.x = -scale.x;
        m_Animator.transform.localScale = scale;*/
    }
}
