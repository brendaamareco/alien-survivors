using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    private Player m_Player;

    public PlayerState(Player player)
    { m_Player = player; }

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

    public virtual PlayerState Equip(Item item)
    {
        item.SetStats(m_Player.GetStats());
        m_Player.SetStats(item);
        //TODO: poner logica de agregar a la lista, verificar si se puede agregar

        return this;
    }

    public virtual PlayerState Equip(Weapon weapon)
    {
        weapon.SetStats(m_Player.GetStats());
        m_Player.SetStats(weapon);
        //TODO: poner logica de agregar a la lista, verificar si se puede agregar

        return this;
    }

    protected Player GetPlayer()
    { return m_Player; }
}
