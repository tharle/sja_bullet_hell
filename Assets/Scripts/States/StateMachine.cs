using System;
using System.Collections.Generic;
using UnityEngine;
using TypeReferences;
using UnityEngine.XR;
using Unity.VisualScripting;

public class StateMachine : MonoBehaviour
{
    [SerializeField] Animator m_Animator;

    // https://github.com/SolidAlloy/ClassTypeReference-for-Unity
    [SerializeField, Inherits(typeof(State))] protected TypeReference m_DefaultState;

    [SerializeReference, SubclassSelector] protected List<State> m_States;
    protected State m_CurrentState;

    Rigidbody2D m_Rigidbody;

    public virtual void Awake()
    {
        foreach (var state in m_States)
        {
            state.Owner = this;
        }
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        State state = GetState(m_DefaultState.Type);
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

    public bool ChangeState(State newState, params State.Param[] stateParams)
    {
        if (newState == null) return false;

        if (!CanChangeState(newState)) return false;

        m_CurrentState?.OnExit();
        m_CurrentState = newState;
        m_CurrentState?.SetParams(stateParams);
        m_CurrentState?.OnEnter();

        return true;
    }

    public bool ChangeState<T>(params State.Param[] stateParams)
    {
        State newState = GetState<T>();
        return ChangeState(newState, stateParams);
    }

    public bool CanChangeState(State newState)
    {
        if (m_CurrentState == null || m_CurrentState == newState) return true;

        if (m_CurrentState != null) return m_CurrentState.Rules is not EStateRules.UnCancellable;

        return true;
    }

    public State GetState(Type type)
    {
        foreach (var state in m_States)
            if (state.GetType() == type) return state;

        return null;
    }

    public State GetState<T>()
    {
        foreach (var state in m_States)
            if (state is T) return state;

        return null;
    }

    public void Move(Vector2 m_Direction)
    {
        //transform.forward = m_Direction;
        m_Rigidbody.velocity = m_Direction * 0.2f; // TODO change for entity 
        m_Animator.SetFloat(GameParameters.AnimationEnemy.FLOAT_VELOCITY, 1f);
    }

    public void Stop()
    {
        m_Rigidbody.velocity = Vector2.zero;
        m_Animator.SetFloat(GameParameters.AnimationEnemy.FLOAT_VELOCITY, 0);
    }
}

[Flags]
public enum EStateRules
{
    AcceptInputs = 1,
    UnCancellable = 2
}

[Serializable]
public abstract class State
{
    [HideInInspector] public StateMachine Owner;

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

    // changer par enum apres
    [SerializeField] private string m_Id;
    [SerializeField] private EStateRules m_Rules;


    public string Id => m_Id;
    public EStateRules Rules => m_Rules;

    public virtual void SetParams(params Param[] stateParams) { }

    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    public virtual void OnCancel() { }
    public virtual void OnExit() { }

}
