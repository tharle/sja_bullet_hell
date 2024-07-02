using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_Body;
    private BulletLoader m_BulletLoader;

    private float m_MultPixel = 0.16f; // mult fom grid layout
    private float m_Speed = 5f;
    private int m_Damage;
    private Color m_Color;
    private Entity m_Owner;
    private List<ItemEffect> m_WallEffects = new List<ItemEffect>();

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody2D>();
        m_BulletLoader = BulletLoader.Instance;
    }

    public void SetBulllet(Vector2 _direction, Entity _owner, Vector3 _position ,List<ItemEffect> _wallEffects = null)
    {
        m_Owner = _owner;
        m_Color = _owner.EntityColor;
        GameObject currentBullet = m_BulletLoader.Get(_owner.CurrentBullet);
        currentBullet.transform.parent = transform;
        currentBullet.transform.position = _position;
        m_WallEffects = _wallEffects;
        m_Damage = _owner.Stats.Damage;
        m_Speed = _owner.Stats.BulletSpeed * m_MultPixel;       
        // Le .16 est pas aleatoire, il est la tail en pixel de ma grid. 16x16, je vais un Classe de config apres
        transform.Translate(_direction * 0.16f, Space.World);  // Translate un "carré" pour que le balle spawn dans l'arme
        m_Body.AddForce(_direction*m_Speed,ForceMode2D.Impulse);
    }

    private void HitWall()
    {
        if (m_WallEffects != null)
        {
            foreach (var effect in m_WallEffects)
            {
                if (effect is BulletEffect bulletEffect)
                    bulletEffect.Execute(m_Owner, this);
                else
                    effect.Execute(m_Owner);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            HitWall();
        else if (_collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if(damageable is Entity entity && entity == m_Owner)
                return;

            damageable.TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
