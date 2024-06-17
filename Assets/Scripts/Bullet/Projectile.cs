using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Bullet outerShell;
    private Rigidbody2D rb;

    private float speed = 15f;
    private int damage;
    private Color color;
    private Entity owner;

    private List<ItemEffect> wallEffects = new List<ItemEffect>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetBulllet(Vector2 _direction, Entity _owner, List<ItemEffect> _wallEffects = null)
    {
        owner = _owner;
        color = _owner.EntityColor;
        outerShell = Instantiate(outerShell);
        outerShell.ChangeColor(color);
        outerShell.transform.parent = transform;
        wallEffects = _wallEffects;
        damage = _owner.Stats.damage;
        speed = _owner.Stats.bulletSpeed;

        rb.AddForce(_direction*speed,ForceMode2D.Impulse);
    }

    private void HitWall()
    {
        if (wallEffects != null)
        {
            foreach (var effect in wallEffects)
            {
                if (effect is BulletEffect bulletEffect)
                    bulletEffect.Execute(owner, this);
                else
                    effect.Execute(owner);
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
            if(damageable is Entity entity && entity == owner)
                return;

            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
