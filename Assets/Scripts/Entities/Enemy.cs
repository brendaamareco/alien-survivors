using System;
using UnityEngine;

public class Enemy : DamageableEntity
{
    private Motion m_Motion;
    private Weapon m_Weapon;

    private void Start()
    {
        m_Weapon = GetComponentInChildren<Weapon>();
        m_Motion = GetComponentInChildren<Motion>();

        m_Weapon.SetStats(GetStats());
        SetStats(m_Weapon);
    }

    public void Move(Vector3 vector)
    { m_Motion.Move(vector, GetSpeedPoints()); }

    public void Attack(Vector3 target)
    { m_Weapon.Attack(GetAttackPoints(), target); }

    public bool CanAttack()
    {
        return m_Weapon.CanAttack();
    }

    public void Rotate(Vector3 target)
    {
        Vector3 targetDirection = target - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public static implicit operator Enemy(GameObject v)
    {
        throw new NotImplementedException();
    }
}
