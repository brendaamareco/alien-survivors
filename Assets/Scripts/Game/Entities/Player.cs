using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : DamageableEntity
{
    [SerializeField] Motion m_Motion;
    [SerializeField] Weapon m_DefaultWeapon;

    private List<Item> m_Items;
    private List<Weapon> m_Weapons;
    private int m_Experience;

    private void Awake()
    {
        m_Items = new List<Item>();

        m_DefaultWeapon.SetStats(GetStats());
        SetStats(m_DefaultWeapon);

        m_Weapons = new() { m_DefaultWeapon };
        m_Experience = 0;
    }

    public void Move(Vector3 vectorMovement)
    {

    }

    public void Attack(Vector3 target)
    { m_Weapons.ForEach(weapon => weapon.Attack(GetAttackPoints(), target)); }

    public void Equip(Item item)
    {
        item.SetStats(GetStats());
        SetStats(item);
        //TODO: poner logica de agregar a la lista, verificar si se puede agregar
    }

    public void Equip(Weapon weapon)
    {
        weapon.SetStats(GetStats());
        SetStats(weapon);
        //TODO: poner logica de agregar a la lista, verificar si se puede agregar
    }

    public List<Weapon> GetWeapons() 
    { return m_Weapons; }

    public List<Item> GetItems() 
    { return m_Items;}
}
