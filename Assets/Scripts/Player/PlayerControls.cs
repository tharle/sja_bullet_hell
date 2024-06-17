using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private PlayerAnimation m_PlayerAnimation;
    private Weapon m_Weapon;
    private Rigidbody2D m_Rigidbody;

    private PlayerEntity m_PlayerEntity;

    private bool m_IsLockInputs;
    private bool m_IsDashing;
    private float m_FireRateTimer;

    public bool CanShoot => !m_IsDashing;
    public bool CanDash => !m_IsDashing;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerAnimation = GetComponentInChildren<PlayerAnimation>();
        m_Weapon = GetComponentInChildren<Weapon>();
        m_PlayerEntity = GetComponent<PlayerEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if (m_IsLockInputs || m_IsDashing)
            return;

        Move();

        // TODO verifier si je vais avoir du dashing
        /* if (CanDash && Input.GetKeyDown(KeyCode.Space))
             Dash();*/
        if(m_FireRateTimer >= 0) m_FireRateTimer -= Time.deltaTime;

        if (CanShoot && Input.GetKey(KeyCode.Mouse0))
            Shoot();
    }

    private void Shoot()
    {
        if (m_FireRateTimer <= 0)
        {
            m_FireRateTimer = 1f / m_PlayerEntity.Stats.bulletPerSeconds;

            GameObject bulletGO = Instantiate(m_PlayerEntity.BulletPrefab, m_Weapon.transform.position, Quaternion.identity);
            if (bulletGO.TryGetComponent<Projectile>(out var projectile))
            {
                var wallEffect = new List<ItemEffect>();
                foreach (var item in m_PlayerEntity.Items)
                    wallEffect.AddRange(item.wallEffects);

                projectile.SetBulllet(m_Weapon.Direction, m_PlayerEntity, wallEffect);
                m_PlayerEntity.HasShoot();
            }
        }
    }

    private void Move()
    {
        float axisH = Input.GetAxis("Horizontal");
        float axisV = Input.GetAxis("Vertical");

        Vector2 velocity = Vector2.zero;
        velocity.x = m_Speed * axisH;
        velocity.y = m_Speed * axisV;

        m_Rigidbody.velocity = velocity;
        m_PlayerAnimation.AddVelocity(velocity.magnitude);
    }
}
