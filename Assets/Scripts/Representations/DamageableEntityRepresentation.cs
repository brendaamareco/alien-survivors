using System;
using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent (typeof(Animator))]
public class DamageableEntityRepresentation : MonoBehaviour
{
    [SerializeField] string animationNameAttack = "";

    [SerializeField] GameObject stunnedVfx;
    [SerializeField] GameObject confusedVfx;

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
        GameEventManager.GetInstance().Suscribe(GameEvent.STUNNED, HandleStunned);
        GameEventManager.GetInstance().Suscribe(GameEvent.PLAYER_DEFAULT_STATE, HandleDefaultState);
        GameEventManager.GetInstance().Suscribe(GameEvent.CONFUSED, HandleConfused);
    }

    
    private void HandleDefaultState(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            m_Animator.SetBool("IsStunned", false);
            m_Animator.SetBool("IsConfused", false);

            if (stunnedVfx)
                stunnedVfx.SetActive(false);
            if (confusedVfx)
                confusedVfx.SetActive(false);
        }
    }

    private void HandleConfused(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            m_Animator.SetBool("IsConfused", true);

            if (confusedVfx)
                confusedVfx.SetActive(true);
        }
    }

    private void HandleStunned(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            m_Animator.SetBool("IsStunned", true);

            if (stunnedVfx)
                stunnedVfx.SetActive(true);
        }
    }

    private void HandleDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
            m_Animator.SetTrigger("ReceiveDamage");
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
