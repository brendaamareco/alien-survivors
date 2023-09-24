using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : SpecialAreaBehaviour
{
    [SerializeField][Range(0, 1)] float healPercentage = 0f;
    
    public override void HandlePlayerExitArea(Player player)
    { }

    public override void HandlePlayerInArea(Player player)
    {   
        player.Heal(player.GetCurrentHealthPoints() + player.GetCurrentHealthPoints() * healPercentage);
        Destroy(gameObject);
    }
}
