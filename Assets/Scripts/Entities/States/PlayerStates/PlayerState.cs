using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    private Player m_Player;

    public PlayerState(Player player)
    { 
        m_Player = player;
        GameEventManager.GetInstance().Publish(GameEvent.STATE_DEFAULT, new EventContext(player));
    }

    public virtual PlayerState Move(Vector3 vectorMovement, Motion motion)
    {
        motion.Move(vectorMovement, m_Player.GetSpeedPoints());
        return this;
    }

    public virtual PlayerState Attack(Vector3 target)
    { 
        m_Player.GetWeapons().ForEach(weapon => weapon.Attack(m_Player.GetAttackPoints(), target));
        return this;
    }

    protected Player GetPlayer()
    { return m_Player; }
}
