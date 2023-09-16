using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : DamageableEntity
{
    [SerializeField] Motion motion;
        
    private int m_Experience;
    private List<InventorySlot<Weapon>> m_WeaponInventory;
    private List<Item> m_Items;
    private int m_MaxElementsInInventory;
    private PlayerState m_State;

    private void Awake()
    {
        m_Items = new();
        m_WeaponInventory = LoadWeaponInventory();
        m_State = new PlayerState(this);
        m_Experience = 0;
        m_MaxElementsInInventory = 6;
    }

    public void Move(Vector3 vectorMovement)
    { m_State = m_State.Move(vectorMovement, motion); }

    public void Attack(Vector3 target)
    { m_State = m_State.Attack(target); }

    public void Equip(Item item)
    { 
    
    }

    public void Equip(Weapon weapon)
    { 
        if (m_WeaponInventory.Count < m_MaxElementsInInventory)
        {
            foreach(InventorySlot<Weapon> weaponSlot in m_WeaponInventory)
            {
                if (weaponSlot.IsFree())
                {
                    weapon.SetStats(GetStats());
                    SetStats(weapon.GetStats());

                    weapon.gameObject.layer = gameObject.layer;
                    weaponSlot.SetElement(weapon);
                }
            }
        }
    }

    public int GetMaxElementsInInventory()
    { return m_MaxElementsInInventory; }

    public List<Weapon> GetWeapons() 
    { 
        List<Weapon> weapons = new();

        foreach(InventorySlot<Weapon> weaponSlot in m_WeaponInventory) 
        {
            if (!weaponSlot.IsFree())
                weapons.Add(weaponSlot.GetElement());
        }

        return weapons; 
    }

    public List<Item> GetItems() 
    { return m_Items;}

    public int GetExperience()
    { return m_Experience; }

    public void AddExperience(int experience)
    { m_Experience += experience; }


    public List<InventorySlot<Weapon>> LoadWeaponInventory()
    {
        List<InventorySlot<Weapon>> weaponInventory = new();
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            InventorySlot<Weapon> slot = new(weaponSlots[i].transform);            
            Weapon weapon = weaponSlots[i].GetComponentInChildren<Weapon>();

            if (weapon)
            {
                weapon.SetStats(GetStats());
                SetStats(weapon);

                weapon.gameObject.layer = gameObject.layer;
                slot.SetElement(weapon);
            }

            weaponInventory.Add(slot);
        }

        return weaponInventory;
    }
}
