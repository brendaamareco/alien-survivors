using System;
using UnityEngine;

[Serializable]
public class Stats 
{
    [SerializeField] private int m_Attack = 0;
    [SerializeField] private int m_Defense = 0;
    [SerializeField] private int m_Health = 0;
    [SerializeField] private int m_Speed = 0;

    public int Attack { get => m_Attack; set => m_Attack = value; }
    public int Defense { get => m_Defense; set => m_Defense = value; }
    public int Health { get => m_Health; set => m_Health = value; }
    public int Speed { get => m_Speed; set => m_Speed = value; }
}
