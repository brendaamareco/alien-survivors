using CodeMonkey.HealthSystemCM;
using UnityEngine;

[RequireComponent(typeof(BaseStats))]
public class DamageableEntity : MonoBehaviour, IEntity
{
    [SerializeField] private string damageableName;

    private Stats m_Stats;
    private HealthSystem m_HealthSystem;

    private void Awake()
    {
        ResetStats();
    }

    private void OnEnable()
    {
        ResetStats();

    }

    private void Start()
    {
        ResetStats();

    }

    protected virtual void Update()
    {
        if (m_HealthSystem.GetHealthMax() != GetMaxHealthPoints())
            SetMaxHealth(GetMaxHealthPoints());
    }

    public void ResetStats()
    {
        m_Stats = gameObject.GetComponent<BaseStats>();
        m_HealthSystem = new HealthSystem(m_Stats.GetHealth());
        m_HealthSystem.OnDead += HealthSystem_OnDead;
    }

    public void Heal(float amount)
    { 
        m_HealthSystem.Heal(amount);
        GameEventManager.GetInstance().Publish(GameEvent.HEALED, new EventContext(this));
    }

    public void ReceiveDamage(float amount)
    {     
        m_HealthSystem.Damage(Mathf.Max(0, amount - GetDefensePoints()));

        if (m_HealthSystem.GetHealth() > 0)
            GameEventManager.GetInstance().Publish(GameEvent.DAMAGE, new EventContext(this));
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
    { 
        if (m_Stats == null)
            m_Stats = gameObject.GetComponent<BaseStats>();

        return m_Stats.GetAttack();  
    }

    public virtual int GetDefensePoints()
    { return m_Stats.GetDefense(); }

    public virtual int GetMaxHealthPoints()
    { return m_Stats.GetHealth(); }

    public virtual int GetSpeedPoints()
    { return m_Stats.GetSpeed(); }    

    public HealthSystem GetHealthSystem() 
    { return m_HealthSystem; }

    public string GetName()
    { return damageableName; }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    { GameEventManager.GetInstance().Publish(GameEvent.DEAD, new EventContext(this)); }

    private void OnDisable()
    { 
        if (m_HealthSystem != null)
            m_HealthSystem.OnDead -= HealthSystem_OnDead;
    }

    public virtual void AcceptWeaponComponent(WeaponComponent weaponComponent)
    { weaponComponent.HandleOnHit(this); }
}
