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
        m_BarValue.gameObject.SetActive(false);
    }

    private void OnChangeAndShow(float ratio)
    {
        StopAllCoroutines();
        m_BarValue.gameObject.SetActive(true);
        m_BarValue.value = ratio;
        StartCoroutine(HideAfterSomeTimeRoutine());
    }

    IEnumerator HideAfterSomeTimeRoutine()
    {
        yield return new WaitForSeconds(GameParameters.Prefs.UI_LIFE_BAR_DISPLAY_TIME);
        OnHideBar();
    }
}
