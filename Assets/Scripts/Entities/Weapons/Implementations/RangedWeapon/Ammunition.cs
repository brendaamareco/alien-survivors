using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent (typeof(Rigidbody))]
public class Ammunition : MonoBehaviour, IEntity
{
    [SerializeField] private float distance = 0;
    [SerializeField] WeaponComponent weaponComponent;
    private Weapon weapon;
    
    private int m_DamagePoints;
    private BoxCollider m_BoxCollider;
    private Vector3 m_OriginPosition;
    private Rigidbody m_Rigidbody;
    private bool IsApplyingEffect = false;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = false;

        m_OriginPosition = transform.position;

        if (weaponComponent)
            GameEventManager.GetInstance().Suscribe(GameEvent.WEAPON_COMPONENT_END, HandleWeaponComponentEnd);
    }

    private void HandleWeaponComponentEnd(EventContext context)
    {
        WeaponComponent other = (WeaponComponent)context.GetEntity();
        if (other == weaponComponent)
            Destroy(gameObject);
    }

    private void Update()
    {

        float currentDistance =  Vector3.Distance(transform.position, m_OriginPosition);

        if (currentDistance > distance)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {  
        DamageableEntity damageable = collision.gameObject.GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
        {
            if (m_Rigidbody  != null)
            {
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.useGravity = true;           
            }

            GameEventManager.GetInstance().Publish(GameEvent.ATTACK, new EventContext(this.weapon));
            damageable.ReceiveDamage(m_DamagePoints);

            if (weaponComponent && !IsApplyingEffect)
            {
                damageable.AcceptWeaponComponent(weaponComponent);
                IsApplyingEffect = true;
            }
        }
        
        if (!IsApplyingEffect)
            Destroy(gameObject);
    }

    public void SetDamagePoints(int damagePoints)
    { m_DamagePoints = damagePoints; }

    public void SetLayerMask(LayerMask layer)
    { gameObject.layer = layer; }

    public void SetDistance(float newDistance)
    { distance = newDistance; }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public string GetName()
    { return typeof(Ammunition).Name; }
}
