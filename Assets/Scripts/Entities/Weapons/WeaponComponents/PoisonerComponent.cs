using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonerComponent : WeaponComponent
{
    [SerializeField] float damageAmount;

    private bool m_IsCoroutineRunning = false;

    public override void HandleOnHit(DamageableEntity damageable) {}

    public override void HandleOnHit(Player player)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < GetProbability() && !m_IsCoroutineRunning)
            StartCoroutine(nameof(PoisonCoroutine), player);
        else
            GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }

    private IEnumerator PoisonCoroutine(Player player)
    {
        GameEventManager.GetInstance().Publish(GameEvent.PLAYER_POISONED, new EventContext(player));
        m_IsCoroutineRunning = true;
   
        for (int i = 0; i < GetDuration(); i++)
        {
            player.ReceiveDamage(damageAmount);
            yield return new WaitForSeconds(1f);
        }

        m_IsCoroutineRunning = false;
        GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }
}
