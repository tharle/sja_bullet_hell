using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    [SerializeField] private Transform m_Limit1;
    [SerializeField] private Transform m_Limit2;


    private static Limits m_Instance;
    public static Limits Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }

    public void GetLimits(out Vector2 max, out Vector2 min)
    {
        max = new Vector2();
        min = new Vector2();
        if (m_Limit1.position.x >= m_Limit2.position.x)
        {
            max.x = m_Limit1.position.x;
            min.x = m_Limit2.position.x;
        }else
        {
            max.x = m_Limit2.position.x;
            min.x = m_Limit1.position.x;
        }

        if (m_Limit1.position.y >= m_Limit2.position.y)
        {
            max.y = m_Limit1.position.y;
            min.y = m_Limit2.position.y;
        }
        else
        {
            max.y = m_Limit2.position.y;
            min.y = m_Limit1.position.y;
        }
    }

    public Vector2 Clamp(Vector2 position)
    {
        
        GetLimits(out Vector2 max, out Vector2 min);
        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);

        return position;
    }
}
