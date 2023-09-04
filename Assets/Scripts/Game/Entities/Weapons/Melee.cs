
using UnityEngine;

public class Melee : Weapon
{
    public override void PerformAttack(float attackBase)
    {
        foreach (DamageableEntity damageable in GetDamageablesInArea())
            damageable.ReceiveDamage(attackBase + GetWeaponAttackPoints());
    }
}
