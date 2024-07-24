using System.Collections.Generic;
using UnityEngine;
using TypeReferences;
using System;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private EnemyAnimation m_Animator;
    public EnemyAnimation EnemyAnimation => m_Animator;
    private bool m_LookingLeft = true;

    private EnemyEntity m_Enemy;
    public EnemyEntity Enemy => m_Enemy;

    // https://github.com/SolidAlloy/ClassTypeReference-for-Unity
    [SerializeField, Inherits(typeof(AEnemyState))] private TypeReference m_DefaultState;

    [SerializeReference, SubclassSelector] private List<AEnemyState> m_States;
    private AEnemyState m_CurrentState;

    Rigidbody2D m_Rigidbody;

    public virtual void Awake()
    {
        foreach (var state in m_States)
        {
            state.setOwner(this);
        }
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Enemy = GetComponent<EnemyEntity>();

        AEnemyState state = GetState(m_DefaultState.Type);
        if (state != null) ChangeState(state);

        SubscribeAll();
    }

    private void SubscribeAll()
    {
        m_Enemy.OnDead += OnDead;
    }

    private void OnDead()
    {
        ChangeState<EnemyDeadState>();
    }

    private void Update()
    {
        m_CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        m_CurrentState?.FixedUpdate();

        // That make all enemies be inside of ARENA
        transform.position = Limits.Instance.Clamp(transform.position);
    }

    private void OnDestroy()
    {
        m_CurrentState?.OnCancel();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable is EnemyEntity) return; // No more friend fire =(

            damageable.TakeDamage(m_Enemy.Stats.Damage);
            StartCoroutine(StopAfterRoutine());
        }
    }

    private IEnumerator StopAfterRoutine()
    {
        yield return new WaitForSeconds(m_Enemy.Fatigue);
        ChangeState<EnemyIdleState>();
    }

    public bool ChangeState(AEnemyState newState, params AEnemyState.Param[] stateParams)
    {
        if (newState == null) return false;

        if (!CanChangeState(newState)) return false;

        m_CurrentState?.OnExit();
        m_CurrentState = newState;
        m_CurrentState?.SetParams(stateParams);
        m_CurrentState?.OnEnter();

        return true;
    }

    public bool ChangeState<T>(params AEnemyState.Param[] stateParams)
    {
        AEnemyState newState = GetState<T>();
        return ChangeState(newState, stateParams);
    }

    public bool CanChangeState(AEnemyState newState)
    {
        if (m_CurrentState == null || m_CurrentState == newState) return true;

        if (m_CurrentState != null) return m_CurrentState.Rules is not EEnemyStateRules.UnCancellable;

        return true;
    }

    private AEnemyState GetState(System.Type type)
    {
        foreach (var state in m_States)
            if (state.GetType() == type) return state;

        return null;
    }

    public AEnemyState GetState<T>()
    {
        foreach (var state in m_States)
            if (state is T) return state;

        return null;
    }

    public void Move(Vector2 direction)
    {
        m_Rigidbody.velocity = direction * 0.2f; // TODO change for entity 
        m_Animator.ChangeVelocity(m_Rigidbody.velocity.magnitude);
        Flip(direction);
    }

    public void MoveToPlayer(int multVelocity = 1)
    {
        Move(DirectionToPlayer() * multVelocity);
    }

    public Vector2 DirectionToPlayer()
    {
        PlayerEntity player = GameManager.Instance.PlayerEntity;
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        return direction;
    }

    private void Flip(Vector2 direction)
    {
        if (direction.x < 0 && !m_LookingLeft) return;
        if (direction.x > 0 && m_LookingLeft) return;

        m_LookingLeft = !m_LookingLeft;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Stop()
    {
        m_Rigidbody.velocity = Vector2.zero;
        m_Animator.ChangeVelocity(0.0f);
    }

    public void CheckPlayerInTauntRange()
    {
        if (m_Enemy.IsAlive() && DistanceFromPlayer() <= m_Enemy.TautDistance)
            ChangeState<EnemyChaseState>();
    }

    public float DistanceFromPlayer()
    {
        Vector2 playerPosition = GameManager.Instance.PlayerEntity.transform.position;
        return Vector2.Distance(transform.position, playerPosition);
    }

    public bool IsInAttackRange()
    {
        return DistanceFromPlayer() <= m_Enemy.AttackRange;
    }

    public void Shoot()
    {
        GameObject bulletGO = Instantiate(m_Enemy.BulletPrefab, transform.position, Quaternion.identity);
        if (bulletGO.TryGetComponent<Projectile>(out var projectile))
        {
            var wallEffect = new List<ItemEffect>();
            foreach (var item in m_Enemy.Items)
                wallEffect.AddRange(item.wallEffects);
            Vector2 direction = DirectionToPlayer();
            projectile.SetBulllet(direction, m_Enemy, transform.position, wallEffect);

            if (m_Enemy.IsElite) projectile.transform.localScale *= 2.5f;

            Flip(direction);

            AudioManager.Instance.Play(EAudio.SFXFishingRod, transform.position);
            m_Enemy.HasShoot();
            if(m_Enemy.Type == EEnemy.Chicken) AudioManager.Instance.Play(EAudio.SFXChicken);
            else AudioManager.Instance.Play(EAudio.SFXCow);
            
        }
    }

    public void DestroyIt(float time)
    {
        if(gameObject.TryGetComponent<BoxCollider2D>(out var collider)) Destroy(collider);
        Destroy(gameObject, time);
        EffectManager.Instance.CastEffect(EEffect.Unsummon, transform.position, m_Enemy.ColorDie, time);

        TrySpawItem(time + 0.5f);
    }

    private void TrySpawItem(float delay)
    {
        int chance = UnityEngine.Random.Range(0, 100);

        if(chance <= GameParameters.Prefs.ITEM_DROP_CHANCE) DropRandomItem(delay);
    }

    private void DropRandomItem(float delay)
    {
        ItemLoader.Instance.DropRandomItem(transform.position, delay);
    }
}

[Flags]
public enum EEnemyStateRules
{
    UnCancellable = 1
}

[Serializable]
public abstract class AEnemyState
{
    protected EnemyStateMachine m_Owner;

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

    [SerializeField] private EEnemyStateRules m_Rules;
    public EEnemyStateRules Rules => m_Rules;

    public virtual void SetParams(params Param[] stateParams) { }

    public virtual void OnEnter() { m_Owner.StopAllCoroutines();}
    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    public virtual void OnCancel() { }
    public virtual void OnExit() { }

    public void setOwner(EnemyStateMachine owner)
    {
        m_Owner = owner;
    }
}
