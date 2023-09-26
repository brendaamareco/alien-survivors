using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(SphereCollider))]
public abstract class Weapon : StatsDecorator, IEntity
{
    [SerializeField] string weaponName;
    [SerializeField] float scope = 10f;
    [SerializeField] float cooldown = 1f;
    [SerializeField] int attackExtraPoints = 0;
    [SerializeField] float upgradePercentage = 0.25f;
    [SerializeField] int maxLevel = 1;
    [SerializeField] WeaponComponent weaponComponent;

    private bool m_IsAttacking = false;
    private int m_Level;
    private SphereCollider m_ScopeCollider;
    private HashSet<DamageableEntity> m_DamageablesInArea;

    private void Start()
    {
        m_ScopeCollider = GetComponent<SphereCollider>();
        m_ScopeCollider.isTrigger = true;
        SetScope(scope);

        m_DamageablesInArea = new();
        m_Level = 1;
    }

    public void Attack(int attack, Vector3 target)
    {
        if (!m_IsAttacking)      
            StartCoroutine(nameof(AttackCoroutine), attack);
    }

    private IEnumerator AttackCoroutine(int attack)
    {
        m_IsAttacking = true;
        PerformAttack(attack);

        yield return new WaitForSeconds(cooldown);

        m_IsAttacking = false;
    }

    public abstract void PerformAttack(int attack);

    public void SetScope(float scope)
    { 
        this.scope = scope;
        m_ScopeCollider.radius = this.scope;
        m_ScopeCollider.center = Vector3.zero;
    }

    public void Upgrade() { }

    protected HashSet<DamageableEntity> GetDamageablesInArea() 
    { return m_DamageablesInArea;}

    public string GetName()
    { return weaponName; }

    public override int GetAttack()
    { return GetStats().GetAttack() + attackExtraPoints; }

    public override int GetDefense()
    { return GetStats().GetDefense(); }

    public override int GetHealth()
    { return GetStats().GetHealth(); }

    public override int GetSpeed()
    { return GetStats().GetSpeed(); }

    public float GetScope()
    { return scope; }

    public WeaponComponent GetWeaponComponent()
    { return weaponComponent; }

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
}
