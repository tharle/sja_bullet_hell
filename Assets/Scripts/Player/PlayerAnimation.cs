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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            DoFlip();
        }
    }

    public void AddVelocity(float velocity)
    {
        m_Animator.SetFloat("velocity", velocity);
    }

    public void DoFlip()
    {
        m_IsLookRight = !m_IsLookRight;
        //StopAllCoroutines();
        //StartCoroutine(DoFlipRoutine());
        if (m_IsLookRight) m_Animator.SetTrigger("look_to_right");
        else m_Animator.SetTrigger("look_to_left");
    }

    public void OnFlipTo()
    {
        Vector3 scale = transform.parent.localScale;
        scale.x *= -1;
        transform.parent.localScale = scale;
    }

    private IEnumerator DoFlipRoutine()
    {
        Vector3 scaleFrom = transform.parent.localScale;
        Vector3 scaleTo = transform.parent.localScale;
        scaleTo.x = -1 * scaleTo.x;
        yield return null;
        for (int i = 1; i <= 10; i++)
        {
            transform.parent.localScale =  Vector3.Lerp(scaleFrom, scaleTo, i/10);
            yield return new WaitForSeconds(1f);

        }
        //transform.parent.localScale = scale;
    }
}
