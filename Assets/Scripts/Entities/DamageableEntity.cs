using CodeMonkey.HealthSystemCM;
using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BaseStats))]
public class DamageableEntity : MonoBehaviour, IEntity
{
    [SerializeField] private string damageableName;

    private Stats m_Stats;
    private HealthSystem m_HealthSystem;

    protected virtual void Start()
    {
        m_Stats = gameObject.GetComponent<BaseStats>();

        m_HealthSystem = new HealthSystem(m_Stats.GetHealth());
        m_HealthSystem.OnDead += HealthSystem_OnDead; 
    }

    protected virtual void Update()
    {
        if (m_HealthSystem.GetHealthMax() != GetMaxHealthPoints())
            SetMaxHealth(GetMaxHealthPoints());
    }

    public void Heal(float amount)
    { 
        m_HealthSystem.Heal(amount);
        GameEventManager.GetInstance().Publish(GameEvent.HEALED, new EventContext(this));
    }

    public void ReceiveDamage(float amount)
    {
        GameEventManager.GetInstance().Publish(GameEvent.DAMAGE, new EventContext(this));
        m_HealthSystem.Damage(Mathf.Max(0, amount - GetDefensePoints())); 
    }

    private void SetMaxHealth(float maxHealth)
    { m_HealthSystem.SetHealthMax(maxHealth, false); }

    public Stats GetStats()
    { return m_Stats; }

    public void SetStats(Stats stats)
    { m_Stats = stats; }

    public int GetCurrentHealthPoints()
    { return (int)m_HealthSystem.GetHealth(); }

    public float GetCurrentHealthPointsNormalized()
    { return m_HealthSystem.GetHealthNormalized(); }

    public virtual int GetAttackPoints()
    { return m_Stats.GetAttack();  }

    public virtual int GetDefensePoints()
    { return m_Stats.GetDefense(); }

    public virtual int GetMaxHealthPoints()
    { return m_Stats.GetHealth(); }

    public virtual int GetSpeedPoints()
    { return m_Stats.GetSpeed(); }    

    public string GetName()
    { return damageableName; }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    { GameEventManager.GetInstance().Publish(GameEvent.DEAD, new EventContext(this)); }

    private void OnDestroy()
    { m_HealthSystem.OnDead -= HealthSystem_OnDead; }

    public virtual void AcceptWeaponComponent(WeaponComponent weaponComponent)
    { weaponComponent.HandleOnHit(this); }
}
