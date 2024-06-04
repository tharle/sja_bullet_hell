using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private PlayerAnimation m_PlayerAnimation;

    private Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");


        CheckFlipHorizontal(axisH);

        Vector2 velocity = Vector2.zero;
        velocity.x = m_Speed * axisH;
        velocity.y = m_Speed * axisV;

        m_Rigidbody.velocity = velocity;
        m_PlayerAnimation.AddVelocity(velocity.magnitude);
    }

    private void CheckFlipHorizontal(float axisH)
    {
        if (m_PlayerAnimation.IsLookRight && axisH >= 0) return;
        if (!m_PlayerAnimation.IsLookRight && axisH <= 0) return;

        m_PlayerAnimation.DoFlip();
    }
}
