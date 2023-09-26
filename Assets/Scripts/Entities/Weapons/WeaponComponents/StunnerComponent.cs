using System.Collections;
using UnityEngine;

public class StunnerComponent : WeaponComponent
{
    public override void HandleOnHit(DamageableEntity damageable) {}

    public override void HandleOnHit(Player player)
    {
        float randomValue = Random.Range(0f, 1f);
        
        if (randomValue < GetProbability())
            StartCoroutine(nameof(StunCoroutine), player);
    }


    private IEnumerator StunCoroutine(Player player)
    {
        player.SetState(new StunnedPlayerState(player));
        yield return new WaitForSeconds(GetDuration());
        player.SetState(new PlayerState(player));
    }
}
