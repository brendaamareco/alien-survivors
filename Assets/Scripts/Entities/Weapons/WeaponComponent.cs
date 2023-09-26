using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponComponent : MonoBehaviour, IEntity
{
    [SerializeField] float duration;
    [SerializeField][Range(0,1)] float probability;

    public abstract void HandleOnHit(DamageableEntity damageable);
    public abstract void HandleOnHit(Player player);
    public float GetDuration() { return duration; }
    public float GetProbability() {  return probability; }

    public string GetName()
    { return typeof(WeaponComponent).Name; }
}
