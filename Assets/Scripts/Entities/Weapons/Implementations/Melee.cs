
using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] WeaponComponent weaponComponent;

    public override void PerformAttack(int attack)
    {
        foreach (DamageableEntity damageable in GetDamageablesInArea())
        {
            GameEventManager.GetInstance().Publish(GameEvent.ATTACK, new EventContext(this));
            damageable.ReceiveDamage(attack);
            
            if (weaponComponent)
                damageable.AcceptWeaponComponent(weaponComponent);
        }
    }
}
