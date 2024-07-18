using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_Body;

    private float m_MultPixel = GameParameters.Prefs.GRID_SIZE_IN_PIXEL;
    private float m_Speed;
    private int m_Damage;
    private float m_Range;
    private Entity m_Owner;
    private List<ItemEffect> m_WallEffects = new List<ItemEffect>();

    private Vector2 m_PosStart;
    

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GetTravelledDistance() >= m_Range) DestroyBullet();
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
        m_Range = owner.Stats.BulletRange * m_MultPixel;
        m_PosStart = transform.position;

        // Translate un "carré" pour que le balle spawn dans l'arme
        transform.Translate(direction * GameParameters.ScreenConfig.DENSITY_PIXELS, Space.World);  
        m_Body.AddForce(direction * m_Speed, ForceMode2D.Impulse);
        
    }

    private float GetTravelledDistance()
    {
        return Vector2.Distance(m_PosStart, transform.position);
    }

    private void HitWall()
    {
        m_WallEffects?.ForEach(TriggerEffect);

        DestroyBullet();
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
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        // TODO add effect destroyed(?)
        Destroy(gameObject);
    }
}
