using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfuserComponent : WeaponComponent
{
    public override void HandleOnHit(DamageableEntity damageable) {}

    public override void HandleOnHit(Player player)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < GetProbability())
            StartCoroutine(nameof(ConfuseCoroutine), player);
    }

    private IEnumerator ConfuseCoroutine(Player player)
    {
        player.SetState(new ConfusedPlayerState(player));
        yield return new WaitForSeconds(GetDuration());
        player.SetState(new PlayerState(player));
    }
}
