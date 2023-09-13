using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject ammunitionPrefab;
    [SerializeField] float ammunitionSpeed = 5f;

    private List<GameObject> m_Ammunitions = new();

    public override void PerformAttack(int attack)
    {
        GameEventManager.GetInstance().Publish(GameEvent.ATTACK, new EventContext(this));

        GameObject ammunitionGameObject = Instantiate(ammunitionPrefab, spawnPosition.position, spawnPosition.rotation);
        Rigidbody ammunitionRb = ammunitionGameObject.GetComponent<Rigidbody>();
        ammunitionRb.MoveRotation(Quaternion.Euler(new Vector3(90, spawnPosition.eulerAngles.y, spawnPosition.eulerAngles.z)));
        ammunitionRb.velocity = spawnPosition.forward * ammunitionSpeed;

        Ammunition ammunition = ammunitionGameObject.GetComponent<Ammunition>();
        ammunition.SetDamagePoints(attack);
        ammunition.SetLayerMask(gameObject.layer);

        m_Ammunitions.Add(ammunitionGameObject);
    } 

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (m_Ammunitions.Contains(other.gameObject))
        {
            m_Ammunitions.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
