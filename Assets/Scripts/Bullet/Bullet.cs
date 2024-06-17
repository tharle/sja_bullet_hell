using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SpriteRenderer m_SpriteRender;

    private void Start()
    {
        m_SpriteRender = GetComponentInChildren<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        if (m_SpriteRender != null) m_SpriteRender.color = color;


    }

   
}
