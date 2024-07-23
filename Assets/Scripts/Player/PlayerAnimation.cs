using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AddVelocity(float velocity)
    {
        m_Animator.SetFloat(GameParameters.AnimationPlayer.FLOAT_VELOCITY, velocity);
    }

    public void PlayWalkSound()
    {
        AudioManager.Instance.Play(EAudio.SFXWalkDirty, transform.position, true, 0.5f);
    }

    public void StopWalkSound()
    {
        AudioManager.Instance.StopAllLooping();
    }

    public void Damage()
    {
        StopCoroutine(OnHitAnimation());
        StartCoroutine(OnHitAnimation());
    }

    IEnumerator OnHitAnimation()
    {
        yield return null;
        m_SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f); 
        m_SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        m_SpriteRenderer.enabled = true;
    }
}
