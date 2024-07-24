using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    private Dictionary<EEffect, Effect> m_Effects;

    private const string m_BundleName = GameParameters.BundleNames.PREFAB_EFFECT;

    #region Singleton
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("Enemy Loader");
                go.AddComponent<EffectManager>();
            }

            return m_Instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
        
        LoadAllEffects();
    }

    private void LoadAllEffects()
    {
        /*m_Effects = new Dictionary<EEffect, Effect>();
        List<Effect> effects = BundleLoader.Instance.LoadAll<Effect, EEffect>(m_BundleName);
        effects.ForEach(effect => m_Effects.Add(effect.Type, effect));*/

        List<GameObject> effects = BundleLoader.Instance.LoadAll<GameObject, EEffect>(m_BundleName);
        m_Effects = effects.ToDictionary(effect => effect.GetComponent<Effect>().Type, effect => effect.GetComponent<Effect>());
    }

    public void CastEffect(EEffect type, Vector2 position, float delay = 0f)
    {
        CastEffect(type, position, Color.white, delay);
    }

    public void CastEffect(EEffect type, Vector2 position, Color color, float delay)
    {
        if (m_Effects == null) m_Effects = new();

        if (!m_Effects.ContainsKey(type))
        {
            Effect newEffect = BundleLoader.Instance.Load<Effect, EEffect>(m_BundleName, type);
            if (newEffect == null) return;

            m_Effects.Add(type, newEffect);
        }

        Effect effect = m_Effects[type];
        StartCoroutine(DoCastEffect(type, position, color, delay));
        
    }

    private IEnumerator DoCastEffect(EEffect type, Vector2 position, Color color, float delay)
    {

        yield return new WaitForSeconds(delay);

        Effect effect = m_Effects[type];
        effect = Instantiate(effect);
        effect.Cast(color);
        effect.transform.position = position;
        AudioManager.Instance.Play(EAudio.SFXExplosion);
    }
}
