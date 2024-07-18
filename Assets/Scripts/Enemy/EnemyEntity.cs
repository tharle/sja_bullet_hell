using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : Entity
{
    // Range for random time for wait in idle after be in patrol
    [SerializeField]  private Vector2 m_IdleTimeRange = new Vector2(0.5f, 2f); // in senconds

    // Range for random distance to patrol after be in idle
    [SerializeField]  private Vector2 m_PatrolTimeRange = new Vector2(0.5f, 2f); // in tiles

    // Distance for start to Chase Enemy
    [SerializeField]  private float m_TautDistance = 6f; // in tiles

    // Time max for chasing, after that the enemy will be in Idle/Patrol
    [SerializeField] private float m_Fatigue = 1f; // in seconds

    public float TautDistance => m_TautDistance * MULTIPLI_PIXEL;

    public float IdleTime { get { return Random.Range(m_IdleTimeRange.x, m_IdleTimeRange.y); } }

    public float PatrolTime { get { return Random.Range(m_PatrolTimeRange.x, m_PatrolTimeRange.y); } }

    public float Fatigue => m_Fatigue;

    public float AttackRange { get { return Stats.BulletRange * MULTIPLI_PIXEL; } }

    public float CooldownAttack { get {
            return 1.0f / Stats.BulletPerSeconds;
        } }
}