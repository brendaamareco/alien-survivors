using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusedPlayerState : PlayerState
{
    public ConfusedPlayerState(Player player) : base(player)
    {
        GameEventManager.GetInstance().Publish(GameEvent.STATE_CONFUSED, new EventContext(player));
    }

    public override PlayerState Move(Vector3 vectorMovement, Motion motion)
    {
        Vector3 newMovement = new Vector3(-vectorMovement.x, vectorMovement.y, -vectorMovement.z);
        return base.Move(newMovement, motion);
    }
}
