using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
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
        m_Animator.SetTrigger(GameParameters.AnimationPlayer.TRIGGER_DAMAGE);
    }
}
