using System;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent(typeof(Animator))]
public class DamageableEntityRepresentation : MonoBehaviour
{
    [SerializeField] string animationNameAttack = "";

    [SerializeField] GameObject stunnedVfx;
    [SerializeField] GameObject confusedVfx;
    [SerializeField] GameObject poisonedVfx;
    [SerializeField] AudioSource damageAudioSource;
    [SerializeField] AudioSource attackAudioSource;

    private DamageableEntity m_Damageable;
    private Weapon m_Weapon;
    private Animator m_Animator;
    private Player m_player;
    private Enemy m_boss;

    void Start()
    {
        m_Damageable = GetComponent<DamageableEntity>();
        m_Animator = GetComponent<Animator>();
        m_Weapon = GetComponentInChildren<Weapon>();
        m_player = GetComponent<Player>();

        GameObject playerObject = GameObject.FindWithTag("Boss");
        if (playerObject != null) {
            m_boss = playerObject.GetComponent<Enemy>();
        }

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleDamage);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleAttack);
        GameEventManager.GetInstance().Suscribe(GameEvent.STATE_DEFAULT, HandleDefaultState);
        GameEventManager.GetInstance().Suscribe(GameEvent.STATE_STUNNED, HandleStunned);
        GameEventManager.GetInstance().Suscribe(GameEvent.STATE_CONFUSED, HandleConfused);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEBUFF_POISONED, HandlePoisoned);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEBUFF_POISONED_END, HandlePoisonedEnd);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleDead);
    }

    private void HandleDead(EventContext context) 
    {
        if (context.GetEntity().Equals(m_player))
            m_Animator.Play("Death");          

        if (context.GetEntity().Equals(m_boss)) {
            Debug.Log("****Boss is dead");
            GameEventManager.GetInstance().Publish(GameEvent.VICTORY, context);
        }
    }

    private void HandlePoisonedEnd(EventContext context)
    {
        if (poisonedVfx)
            poisonedVfx.SetActive(false);
    }

    private void HandlePoisoned(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            if (poisonedVfx)
                poisonedVfx.SetActive(true);
        }
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
        {
            m_Animator.SetTrigger("ReceiveDamage");
            
            if (damageAudioSource)
                damageAudioSource.Play();
        }
    }

    private void HandleAttack(EventContext context)
    {
        if (m_Weapon)
        {
            if (context.GetEntity().Equals(m_Weapon))
            {
                if (!String.IsNullOrEmpty(animationNameAttack))
                    m_Animator.Play(animationNameAttack);

                if (attackAudioSource && !attackAudioSource.isPlaying)
                    attackAudioSource.Play();
            }
        }
    }

    private void OnDeathAnimationEnd()
    {
        GameEventManager.GetInstance().Publish(GameEvent.GAME_OVER, new EventContext(this.m_Damageable));
    }

}
