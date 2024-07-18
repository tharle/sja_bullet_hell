using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    [SerializeField] private Slider m_BarValue;

    private Entity m_Parent;

    private void Start()
    {
        m_Parent = GetComponentInParent<Entity>();
        m_BarValue.gameObject.SetActive(false);
        SubscribeAllAction();
    }

    private void SubscribeAllAction()
    {
        m_Parent.OnHit += ChangeAndShow;
        m_Parent.OnHeal += ChangeAndShow;
    }

    private void ChangeAndShow(float ratio)
    {
        StopAllCoroutines();
        m_BarValue.gameObject.SetActive(true);
        m_BarValue.value = ratio;
        StartCoroutine(HideAfterSomeTimeRoutine());
    }

    IEnumerator HideAfterSomeTimeRoutine()
    {
        yield return new WaitForSeconds(GameParameters.Prefs.UI_LIFE_BAR_DISPLAY_TIME);
        m_BarValue.gameObject.SetActive(false);
    }
}
