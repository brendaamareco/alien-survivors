using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject ammunitionPrefab;
    [SerializeField] float ammunitionTimeLive = 5f;
    [SerializeField] float ammunitionSpeed = 5f;

    public override void PerformAttack(float attackBase)
    {
        GameObject ammunitionGameObject = Instantiate(ammunitionPrefab, spawnPosition.position, spawnPosition.rotation);
        Rigidbody ammunitionRb = ammunitionGameObject.GetComponent<Rigidbody>();
        ammunitionRb.MoveRotation(Quaternion.Euler(new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z)));
        ammunitionRb.velocity = spawnPosition.forward * ammunitionSpeed;

        Ammunition ammunition = ammunitionGameObject.GetComponent<Ammunition>();
        ammunition.SetDamagePoints(attackBase + GetWeaponAttackPoints());
        ammunition.SetTimeAlive(ammunitionTimeLive);

        ammunition.Fire();
    }
}
