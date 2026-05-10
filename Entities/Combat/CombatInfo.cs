namespace Guilder2D;

public class CombatInfo
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    public void TakeDamage(DamageInfo damage)
    {
        CurrentHealth -= damage.Amount;
    }
}