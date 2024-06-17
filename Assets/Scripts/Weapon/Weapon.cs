using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Vector2 m_Direction;
    public Vector2 Direction { get { return m_Direction; } }
    private void Update()
    {
        ChangeDirection();
        //Flip();
    }

    private void ChangeDirection()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = transform.position;
        direction = mousePosition - direction;
        direction = direction.normalized;
        transform.right = direction;
        m_Direction = direction;

    }
}
