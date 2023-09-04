using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject ammunitionPrefab;
    [SerializeField] float ammunitionTimeLive = 5f;

    public override void PerformAttack(float attackBase)
    {
        GameObject ammunitionGameObject = Instantiate(ammunitionPrefab, spawnPosition);
        Ammunition ammunition = ammunitionGameObject.GetComponent<Ammunition>();
        ammunition.SetAttackerLayerMask(GetLayerMask());
        ammunition.SetDamagePoints(attackBase + GetWeaponAttackPoints());
        ammunition.SetTimeAlive(ammunitionTimeLive);

        ammunition.Fire();
    }
}
