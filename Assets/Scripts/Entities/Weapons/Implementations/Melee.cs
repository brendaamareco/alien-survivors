
using UnityEngine;

public class Melee : Weapon
{
    public override void PerformAttack(int attack)
    {
        foreach (DamageableEntity damageable in GetDamageablesInArea())
        {
            GameEventManager.GetInstance().Publish(GameEvent.ATTACK, new EventContext(this));
            damageable.ReceiveDamage(attack);
        }
    }
}
