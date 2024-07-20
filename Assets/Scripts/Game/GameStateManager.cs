using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;


public struct  WaveData
{
    public int Index;
    public int EnnemiesAmount;
    public int EnnemiesDeads;

    public bool AllEnemiesAreDead()
    {
        return EnnemiesDeads >= EnnemiesAmount;
    }
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private List<Transform> m_Spots;
    public List<Transform> Spots => m_Spots;

    public WaveData Wave;


    // https://github.com/SolidAlloy/ClassTypeReference-for-Unity
    [SerializeField, Inherits(typeof(AGameState))] protected TypeReference m_DefaultState;

    [SerializeReference, SubclassSelector] protected List<AGameState> m_States;
    protected AGameState m_CurrentState;

    public virtual void Awake()
    {
        foreach (var state in m_States)
        {
            state.setOwner(this);
        }
    }

    private void Start()
    {
        AGameState state = GetState(m_DefaultState.Type);
        if (state != null) ChangeState(state);
    }

    private void Update()
    {
        m_CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        m_CurrentState?.FixedUpdate();
    }

    private void OnDestroy()
    {
        m_CurrentState?.OnCancel();
    }

    public bool ChangeState(AGameState newState, params AGameState.Param[] stateParams)
    {
        if (newState == null) return false;

        if (!CanChangeState(newState)) return false;

        m_CurrentState?.OnExit();
        m_CurrentState = newState;
        m_CurrentState?.SetParams(stateParams);
        m_CurrentState?.OnEnter();

        return true;
    }

    public bool ChangeState<T>(params AGameState.Param[] stateParams)
    {
        AGameState newState = GetState<T>();
        return ChangeState(newState, stateParams);
    }

    public bool CanChangeState(AGameState newState)
    {
        if (m_CurrentState == null || m_CurrentState == newState) return true;

        if (m_CurrentState != null) return m_CurrentState.Rules is not EGameStateRules.UnCancellable;

        return true;
    }

    private AGameState GetState(System.Type type)
    {
        foreach (var state in m_States)
            if (state.GetType() == type) return state;

        return null;
    }

    public AGameState GetState<T>()
    {
        foreach (var state in m_States)
            if (state is T) return state;

        return null;
    }

    public T Cast<T>(T gameObject) where T : Object
    {
        return Instantiate(gameObject);
    }

}

[System.Flags]
public enum EGameStateRules
{
    UnCancellable = 1
}

[System.Serializable]
public abstract class AGameState
{
    protected GameStateManager m_Owner;

    public struct Param
    {
        public string Key;
        public object Value;

        public Param(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }

    [SerializeField] private EGameStateRules m_Rules;

    public EGameStateRules Rules => m_Rules;

    public virtual void SetParams(params Param[] stateParams) { }

    public virtual void OnEnter() { m_Owner.StopAllCoroutines(); }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    public virtual void OnCancel() { }
    public virtual void OnExit() { }

    public void setOwner(GameStateManager owner)
    {
        m_Owner = owner;
    }
}
