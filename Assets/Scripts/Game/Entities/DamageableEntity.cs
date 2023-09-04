using CodeMonkey.HealthSystemCM;
using UnityEngine;

public class DamageableEntity : MonoBehaviour, IEntity
{
    [SerializeField] private Stats m_Stats;

    private string m_Name;
    private HealthSystem m_HealthSystem;

    protected virtual void Start()
    {
        m_Name = this.transform.name;
        m_HealthSystem = new HealthSystem(m_Stats.Health);
        m_HealthSystem.OnDead += HealthSystem_OnDead;
    }

    public void Heal(float amount)
    { m_HealthSystem.Heal(amount); }

    public void ReceiveDamage(float amount)
    {
        GameEventManager.GetInstance().Publish(GameEvent.DAMAGE, new EventContext(this));
        m_HealthSystem.Damage(Mathf.Max(0, amount - GetDefensePoints())); 
    }

    public void SetMaxHealth(float maxHealth)
    { m_HealthSystem.SetHealthMax(maxHealth, false); }

    public float GetCurrentHealthPoints()
    { return m_HealthSystem.GetHealth(); }

    public virtual float GetAttackPoints()
    { return m_Stats.Attack;  }

    public virtual float GetDefensePoints()
    { return m_Stats.Defense; }

    public virtual float GetMaxHealthPoints()
    { return m_Stats.Health; }

    public virtual float GetSpeedPoints()
    { return m_Stats.Speed; }    

    public string GetName()
    { return m_Name; }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    { GameEventManager.GetInstance().Publish(GameEvent.DEAD, new EventContext(this)); }

    private void OnDestroy()
    { m_HealthSystem.OnDead -= HealthSystem_OnDead; }
}
