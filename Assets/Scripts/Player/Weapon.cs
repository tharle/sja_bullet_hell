using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{

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
        transform.right = direction;

    }
}
