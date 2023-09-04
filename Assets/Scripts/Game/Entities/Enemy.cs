using UnityEngine;

public class Enemy : DamageableEntity
{
    [SerializeField] private Motion m_Motion;
    [SerializeField] private Weapon m_Weapon;

    protected override void Start()
    {
        base.Start();
        m_Weapon = GetComponent<Weapon>();
        m_Motion = GetComponent<Motion>();
    }

    public void Move(Vector3 vector)
    { m_Motion.Move(vector, GetSpeedPoints()); }

    public void Attack(Vector3 target)
    { m_Weapon.Attack(GetAttackPoints(), target); }
}
