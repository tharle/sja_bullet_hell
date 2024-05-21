using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private Animator m_AnimatorBody;

    private Rigidbody2D m_Rigidbody;

    private bool m_IsLookRight;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_IsLookRight = true;
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

        m_AnimatorBody.SetFloat("axis_vertical", axisV);

        FlipHorizontal(axisH);

        Vector2 velocity = Vector2.zero;
        velocity.x = m_Speed * axisH;
        velocity.y = m_Speed * axisV;

        m_Rigidbody.velocity = velocity;
        m_AnimatorBody.SetFloat("velocity_horizontal", velocity.x);
    }

    private void FlipHorizontal(float axisH)
    {
        if (m_IsLookRight && axisH >= 0) return;
        if (!m_IsLookRight && axisH <= 0) return;

        m_IsLookRight = !m_IsLookRight;

        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }
}
