using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedPlayerState : PlayerState
{
    public StunnedPlayerState(Player player) : base(player) {}

    public override PlayerState Attack(Vector3 target)
    { return this; }

    public override PlayerState Move(Vector3 vectorMovement, Motion motion)
    { return this; }
}
