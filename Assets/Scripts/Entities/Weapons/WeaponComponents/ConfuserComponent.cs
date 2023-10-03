using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ConfuserComponent : WeaponComponent
{
    private bool m_IsCoroutineRunning = false;

    public override void HandleOnHit(DamageableEntity damageable) {}

    public override void HandleOnHit(Player player)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < GetProbability() && !m_IsCoroutineRunning)
            StartCoroutine(nameof(ConfuseCoroutine), player);
        else
            GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }

    private IEnumerator ConfuseCoroutine(Player player)
    {
        m_IsCoroutineRunning = true;

        player.SetState(new ConfusedPlayerState(player));
        yield return new WaitForSeconds(GetDuration());
        player.SetState(new PlayerState(player));

        m_IsCoroutineRunning = false;
        GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }
}
