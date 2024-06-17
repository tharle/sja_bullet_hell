using System;

public interface IDamageable
{
    public void TakeDamage(int amount);
    public void Heal(int amount);
    public void Kill();
}
