using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : DamageableEntity
{
    [SerializeField] Motion motion;
    [SerializeField] GameObject[] weaponInventory;
    [SerializeField] GameObject[] itemInventory;

    private int m_Experience;
    private int m_MaxElementsInInventory;
    private PlayerState m_State;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        ResetStats();
        m_MaxElementsInInventory = 6;
        m_State = new PlayerState(this);
        m_Experience = 0;    
    }

    public void Move(Vector3 vectorMovement)
    {
        m_State = m_State.Move(vectorMovement, motion);      
    }

    public void Attack(Vector3 target)
    {
        m_State = m_State.Attack(target);
    }


    public void Equip(Item item)
    {
        foreach (GameObject itemSlotObject in itemInventory)
        {
            if (itemSlotObject.transform.childCount == 0)
            {
                // Instantiate the weapon prefab.
                Item itemInstance = Instantiate(item, Vector3.zero, Quaternion.identity, itemSlotObject.transform);

                // Set the weapon's stats.
                itemInstance.SetStats(GetStats());
                SetStats(itemInstance.GetStats());

                // Set the weapon's layer.
                itemInstance.gameObject.layer = gameObject.layer;

                // Add the weapon to the inventory slot.
                itemInstance.transform.localPosition = Vector3.zero;
                itemInstance.transform.localRotation = Quaternion.identity;

                // Stop the loop.
                break;
            }
        }
    }

    public void Equip(Weapon weapon)
    {
        int weaponSlotIndex = 0;

        foreach (GameObject weaponSlotObject in weaponInventory)
        {
            if (weaponSlotObject.transform.childCount == 0)
            {
                // Instantiate the weapon prefab.
                Weapon weaponInstance = Instantiate(weapon, Vector3.zero, Quaternion.identity, weaponSlotObject.transform);

                // Set the weapon's stats.
                weaponInstance.SetStats(GetStats());
                SetStats(weaponInstance.GetStats());

                // Set the weapon's layer.
                weaponInstance.gameObject.layer = gameObject.layer;

                // Add the weapon to the inventory slot.
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                if (weaponSlotIndex > 0)
                    weaponInstance.AddComponent<AimWeapon>();

                // Stop the loop.
                break;
            }

            weaponSlotIndex++;
        }
    }

    public int GetMaxElementsInInventory()
    { return m_MaxElementsInInventory; }

    public List<Weapon> GetWeapons() 
    { 
        List<Weapon> weapons = new();

        foreach(GameObject weaponSlotObject in weaponInventory)
        {
            if (weaponSlotObject.transform.childCount > 0)
            {
                Weapon weapon = weaponSlotObject.GetComponentInChildren<Weapon>();
                weapons.Add(weapon);
            }
        }

        return weapons; 
    }

    public List<Item> GetItems() 
    {
        List<Item> items = new();

        foreach (GameObject itemSlotObject in itemInventory)
        {
            if (itemSlotObject.transform.childCount > 0)
            {
                Item item = itemSlotObject.GetComponentInChildren<Item>();
                items.Add(item);
            }
        }

        return items;
    }

    public int GetExperience()
    { return m_Experience; }

    public void AddExperience(int experience)
    {
        m_Experience += experience;
        GameEventManager.GetInstance().Publish(GameEvent.EXPERIENCE_CHANGED, new EventContext(this));
    }


    public List<InventorySlot<Weapon>> LoadWeaponInventory()
    {
        List<InventorySlot<Weapon>> weaponInventory = new();
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");

        for (int i = 0; i < m_MaxElementsInInventory; i++)
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

    public List<InventorySlot<Item>> LoadItemInventory()
    {
        List<InventorySlot<Item>> itemInventory = new();
        GameObject[] itemSlots = GameObject.FindGameObjectsWithTag("ItemSlot");

        for (int i = 0; i < m_MaxElementsInInventory; i++)
        {            
            InventorySlot<Item> slot = new(itemSlots[i].transform);
            Item item = itemSlots[i].GetComponentInChildren<Item>();

            if (item)
            {
                item.SetStats(GetStats());
                SetStats(item);

                item.gameObject.layer = gameObject.layer;
                slot.SetElement(item);
            }

            itemInventory.Add(slot);
        }

        return itemInventory;
    }

    public void SetState(PlayerState state)
    { this.m_State = state; }

    public override void AcceptWeaponComponent(WeaponComponent weaponComponent)
    { weaponComponent.HandleOnHit(this); }

    internal T GetComponentInChildren<T>(string v)
    {
        throw new NotImplementedException();
    }
}
