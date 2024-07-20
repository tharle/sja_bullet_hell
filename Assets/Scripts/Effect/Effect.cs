using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffect
{
    Summon,
    Unsummon
} 

public class Effect : MonoBehaviour
{
    [SerializeField] private EEffect m_Effect = EEffect.Summon;
    public EEffect Type => m_Effect;


    public void Cast(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;

        GetComponent<Animator>().SetTrigger(Enum.GetName(typeof(EEffect), m_Effect));
    }

    public void EndAnimation()
    {
        Destroy(gameObject);
    }
}
