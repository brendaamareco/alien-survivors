using System;
using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent (typeof(Animator))]
public class DamageableEntityRepresentation : MonoBehaviour
{
    [SerializeField] string animationNameAttack = "";

    [SerializeField] GameObject stunnedVfx;

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
        GameEventManager.GetInstance().Suscribe(GameEvent.PLAYER_STUNNED, HandleStunned);
        GameEventManager.GetInstance().Suscribe(GameEvent.PLAYER_DEFAULT_STATE, HandleDefaultState);
    }

    private void HandleDefaultState(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            m_Animator.SetBool("IsStunned", false);

            if (stunnedVfx)
                stunnedVfx.SetActive(false);
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
