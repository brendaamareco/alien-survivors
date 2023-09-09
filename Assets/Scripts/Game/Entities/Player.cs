using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : DamageableEntity
{
    [SerializeField] Motion m_Motion;
    [SerializeField] Weapon m_DefaultWeapon;

    private List<Item> m_Items;
    private List<Weapon> m_Weapons;
    private PlayerState m_State;
    private int m_Experience;  

    private void Awake()
    {
        m_Items = new List<Item>();

        m_DefaultWeapon.SetStats(GetStats());
        SetStats(m_DefaultWeapon);

        m_Weapons = new() { m_DefaultWeapon };
        m_Experience = 0;

        m_State = new PlayerState(this);
    }

    public void Move(Vector3 vectorMovement)
    { m_State = m_State.Move(vectorMovement); }

    public void Attack(Vector3 target)
    { m_State = m_State.Attack(target); }

    public void Equip(Item item)
    { m_State = m_State.Equip(item); }

    public void Equip(Weapon weapon)
    { m_State = m_State.Equip(weapon); }

    public List<Weapon> GetWeapons() 
    { return m_Weapons; }

    public List<Item> GetItems() 
    { return m_Items;}
}
