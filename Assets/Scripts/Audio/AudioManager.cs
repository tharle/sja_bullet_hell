using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public enum EAudio
{
    SFXAmbianceSound,
    SFXCard,
    SFXConfirm,
    SFXEnterWellcome,
    SFXFishingRod,
    SFXMiniGameLose,
    SFXMiniGameWin,
    SFXMenuHide,
    SFXMenuShow,
    SFXWalkDirty,
    SFXWinGame
    
}
public class AudioManager
{
    private Dictionary<EAudio, AudioClip> m_AudioClips;
    private AudioPool m_AudioPool;

    private BundleLoader m_Loader;

    private static AudioManager m_Instance;

    public static AudioManager Instance { 
        get {
            if (m_Instance == null) m_Instance = new AudioManager();

            return m_Instance;
        }
    }

    private AudioManager()
    {
        m_AudioPool = new AudioPool();
        m_Loader = BundleLoader.Instance;
        m_AudioClips = m_Loader.LoadSFX();
    }

    public void Play(EAudio audioClipId, Vector3 soundPosition, bool isLooping = false, float volume = 1f)
    {
        AudioSource audioSource;
        audioSource = m_AudioPool.GetAvailable();
        audioSource.clip = m_AudioClips[audioClipId];
        audioSource.transform.position = soundPosition;
        audioSource.volume = volume;

        if (!audioSource.isPlaying) 
        {
            audioSource.Play();
            audioSource.loop = isLooping;
        }
    }


    public void StopAllLooping()
    {
        m_AudioPool.StopAllLooping();
    }
}
