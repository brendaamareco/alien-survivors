using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(SphereCollider))]
public abstract class Weapon : MonoBehaviour, IEntity
{   
    [SerializeField] private float m_Scope = 10f;
    [SerializeField] private float m_Cooldown = 1f;
    [SerializeField] private int m_AttackPoints = 0;

    private bool m_IsAttacking = false;
    private SphereCollider m_ScopeCollider;
    private HashSet<DamageableEntity> m_DamageablesInArea;

    private void Start()
    {
        m_ScopeCollider = GetComponent<SphereCollider>();
        m_ScopeCollider.isTrigger = true;
        SetScope(m_Scope);

        m_DamageablesInArea = new();
    }

    public void Attack(float attackBase, Vector3 target)
    {
        if (!m_IsAttacking)
        {
            SetAim(target);
            StartCoroutine(nameof(AttackCoroutine), attackBase);
        }
    }

    private void SetAim(Vector3 target) 
    {
        Vector3 targetDirection = target - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(transform.position, newDirection, Color.red);
    }

    private IEnumerator AttackCoroutine(float attackBase)
    {
        m_IsAttacking = true;
        PerformAttack(attackBase);

        yield return new WaitForSeconds(m_Cooldown);

        m_IsAttacking = false;
    }

    public abstract void PerformAttack(float attackBase);

    public void SetCooldown(float cooldown)
    { m_Cooldown = cooldown; }

    public void SetScope(float scope)
    { 
        m_Scope = scope;
        m_ScopeCollider.radius = m_Scope;
        m_ScopeCollider.center = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageableEntity damageable = other.GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
            m_DamageablesInArea.Add(damageable);
    }

    private void OnTriggerExit(Collider other)
    {
        DamageableEntity damageable = other.GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
            m_DamageablesInArea.Remove(damageable);
    }

    protected float GetWeaponAttackPoints()
    { return m_AttackPoints; }

    protected HashSet<DamageableEntity> GetDamageablesInArea() 
    { return m_DamageablesInArea;}

    public string GetName()
    { return transform.name; }
}
