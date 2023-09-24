using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReducer : SpecialAreaBehaviour
{
    [SerializeField] [Range(0,1)] float speedReductionPercentage = 0f;

    public override void HandlePlayerExitArea(Player player)
    {
        player.SetState(new PlayerState(player));
    }

    public override void HandlePlayerInArea(Player player)
    {
        player.SetState(new SlowPlayerState(player, speedReductionPercentage));
    }
}
