using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private new String name;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void AddVelocity(float velocity)
    {
        m_Animator.SetFloat("velocity", velocity);
    }
}
