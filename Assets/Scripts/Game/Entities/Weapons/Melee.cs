
public class Melee : Weapon
{
    public override void PerformAttack()
    {
        foreach (DamageableEntity damageable in GetDamageablesInArea())
            damageable.ReceiveDamage(GetAttackPoints());      
    }
}
