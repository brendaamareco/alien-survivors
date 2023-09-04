
using UnityEngine;

public class Melee : Weapon
{
    public override void PerformAttack(float attackBase)
    {
        foreach (DamageableEntity damageable in GetDamageablesInArea())
        {
            GameEventManager.GetInstance().Publish(GameEvent.ATTACK, new EventContext(this));
            damageable.ReceiveDamage(attackBase + GetWeaponAttackPoints());
        }
    }
}
