using System;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject ammunitionPrefab;
    [SerializeField] float ammunitionSpeed = 5f;

    public override void PerformAttack(int attack)
    {
        GameEventManager.GetInstance().Publish(GameEvent.FIRE, new EventContext(this));

        GameObject ammunitionGameObject = Instantiate(ammunitionPrefab, spawnPosition.position, spawnPosition.rotation);
        Rigidbody ammunitionRb = ammunitionGameObject.GetComponent<Rigidbody>();
        ammunitionRb.MoveRotation(Quaternion.Euler(new Vector3(90, spawnPosition.eulerAngles.y, spawnPosition.eulerAngles.z)));
        
        ammunitionRb.velocity = spawnPosition.forward * ammunitionSpeed;

        Ammunition ammunition = ammunitionGameObject.GetComponent<Ammunition>();
        ammunition.SetDamagePoints(attack);
        ammunition.SetLayerMask(gameObject.layer);
        ammunition.SetDistance(GetScope());
    } 
}
