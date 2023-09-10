using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent (typeof(Animator))]
public class DamageableEntityRepresentation : MonoBehaviour
{
    [SerializeField] string animationNameReceiveDamage = "";
    [SerializeField] string animationNameAttack = "";

    private DamageableEntity m_Damageable;
    private Weapon m_Weapon;
    private Animator m_Animator;

    void Start()
    {
        m_Damageable = GetComponent<DamageableEntity>();
        m_Animator = GetComponent<Animator>();
        m_Weapon = GetComponent<Weapon>();

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleDamage);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleAttack);
    }

    private void HandleDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
            m_Animator.Play(animationNameReceiveDamage);
    }

    private void HandleAttack(EventContext context)
    {
        if (m_Weapon != null)
        {
            if (context.GetEntity().Equals(m_Weapon))
                m_Animator.Play(animationNameAttack);
        }
    }

}
