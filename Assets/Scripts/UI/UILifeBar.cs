using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    [SerializeField] Entity m_Parent;
    [SerializeField] private Slider m_BarValue;
    [SerializeField] private bool m_AllwaysDisplay = false;


    private void Start()
    {
        OnHideBar();
        SubscribeAllAction();
    }

    private void SubscribeAllAction()
    {
        m_Parent.OnHit += OnChangeAndShow;
        m_Parent.OnHeal += OnChangeAndShow;
        m_Parent.OnDead += OnHideBar;
    }

    private void OnHideBar()
    {
        if(!m_AllwaysDisplay) m_BarValue.gameObject.SetActive(false);
    }

    private void OnChangeAndShow(float ratio)
    {
        StopAllCoroutines();
        m_BarValue.gameObject.SetActive(true);
        m_BarValue.value = ratio;
        if(!m_AllwaysDisplay) StartCoroutine(HideAfterSomeTimeRoutine());
    }

    IEnumerator HideAfterSomeTimeRoutine()
    {
        yield return new WaitForSeconds(GameParameters.Prefs.UI_LIFE_BAR_DISPLAY_TIME);
        OnHideBar();
    }
}
