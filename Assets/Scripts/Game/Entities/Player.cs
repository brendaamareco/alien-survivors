using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : DamageableEntity
{
    [SerializeField] Motion motion;
    [SerializeField] Weapon defaultWeapon;

    private List<Item> m_Items;
    private List<Weapon> m_Weapons;
    private PlayerState m_State;
    public int m_Experience;  

    private void Awake()
    {
        m_Items = new List<Item>();

        defaultWeapon.SetStats(GetStats());
        SetStats(defaultWeapon);

        m_Weapons = new() { defaultWeapon };
        m_Experience = 0;

        m_State = new PlayerState(this);
    }

    public void Move(Vector3 vectorMovement)
    { m_State = m_State.Move(vectorMovement, motion); }

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
