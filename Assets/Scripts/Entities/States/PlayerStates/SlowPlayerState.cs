using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayerState : PlayerState
{
    private float m_SpeedReductionPercentage;
    public SlowPlayerState(Player player, float speedReductionPercentage) : base(player) 
    { m_SpeedReductionPercentage = speedReductionPercentage; }

    public override PlayerState Move(Vector3 vectorMovement, Motion motion)
    {
        int currentSpeed = GetPlayer().GetSpeedPoints();
        float newSpeed = currentSpeed - currentSpeed * m_SpeedReductionPercentage;

        motion.Move(vectorMovement, (int) newSpeed );
        return this;
    }
}
