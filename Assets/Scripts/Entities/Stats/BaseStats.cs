using System;
using UnityEngine;

[Serializable]
public class BaseStats : Stats
{
    [SerializeField] private int m_Attack = 0;
    [SerializeField] private int m_Defense = 0;
    [SerializeField] private int m_Health = 0;
    [SerializeField] public int m_Speed = 0;

    public override int GetAttack()
    { return  m_Attack; }

    public override int GetDefense()
    { return m_Defense; }

    public override int GetHealth()
    { return m_Health; }

    public override int GetSpeed()
    { return m_Speed; }
}
