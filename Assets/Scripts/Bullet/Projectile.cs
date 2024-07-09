using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_Body;

    private float m_MultPixel = 0.16f; // mult fom grid layout
    private float m_Speed = 5f;
    private int m_Damage;
    private Entity m_Owner;
    private List<ItemEffect> m_WallEffects = new List<ItemEffect>();
    

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }

    public void SetBulllet(Vector2 direction, Entity owner, Vector3 position ,List<ItemEffect> wallEffects = null)
    {
        m_Owner = owner;
        GameObject currentBullet = BulletLoader.Instance.Get(owner.BulletType);
        currentBullet.transform.parent = transform;
        currentBullet.transform.position = position;
        m_WallEffects = wallEffects;
        m_Damage = owner.Stats.Damage;
        m_Speed = owner.Stats.BulletSpeed * m_MultPixel;
        // Translate un "carré" pour que le balle spawn dans l'arme
        transform.Translate(direction * GameParameters.ScreenConfig.DENSITY_PIXELS, Space.World);  
        m_Body.AddForce(direction * m_Speed, ForceMode2D.Impulse);
    }

    private void HitWall()
    {
        m_WallEffects?.ForEach(TriggerEffect);

        Destroy(gameObject);
    }

    private void TriggerEffect(ItemEffect effect)
    {
        if (effect is BulletEffect bulletEffect) bulletEffect.Execute(m_Owner, this);
        else effect.Execute(m_Owner);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) HitWall();
        else if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if(damageable is Entity entity && entity == m_Owner) return;

            damageable.TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }
}
