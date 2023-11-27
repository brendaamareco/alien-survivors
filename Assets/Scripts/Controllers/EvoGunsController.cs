using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvoGunsController : MonoBehaviour
{
    [SerializeField] Player player;

    private List<Weapon> m_WeaponInventory;

    Dictionary<string, string> evoGunstMap = new Dictionary<string, string>
    {
        { "Pistola + Rifle", "Minigun"},
        { "Arco + Katana", "Placeholder" },
    };
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.MAXLVL_WEAPON, HandleMaxLvlWeapon);
    }

    private void HandleMaxLvlWeapon(EventContext context)
    {
        try
        {
            Weapon weapon = (Weapon)context.GetEntity();
            m_WeaponInventory = player.GetWeapons();

            if (m_WeaponInventory.Contains(weapon)) 
            {
                Debug.Log("handlemax level");

                List<Weapon> maxWeapons = GetMaxLevelWeapons(m_WeaponInventory);

                if (maxWeapons.Count >= 2)
                {
                    List<string> evoGuns = FindEvoMatch(maxWeapons);
                    if (evoGuns.Count > 0)
                    {
                        Debug.Log("Evo");
                        EvolveWeapon(evoGuns, player);
                    }
                    else { Debug.Log("No hubo match"); };
                }
                else { Debug.Log("Max weapons menor a 2"); }
            }       
        }
        catch { }  
    }

    // Get a list of weapons at their maximum level
    private List<Weapon> GetMaxLevelWeapons(List<Weapon> weaponList)
    {
        List<Weapon> maxLevelWeapons = new List<Weapon>();

        foreach (Weapon weapon in weaponList)
        {
            Debug.Log(weapon.GetName() + " LVL:" + weapon.GetLevel());
            if (weapon.GetLevel() == weapon.GetMaxLevel())
            {
                maxLevelWeapons.Add(weapon);
            }
        }

        return maxLevelWeapons;
    }
    private List<string> FindEvoMatch(List<Weapon> maxWeapons)
    {
        List<string> evoGuns = new List<string>();
        foreach (Weapon weapon1 in maxWeapons)
        {
            foreach (Weapon weapon2 in maxWeapons)
            {
                string combination = weapon1.GetName() + " + " + weapon2.GetName();
                if (evoGunstMap.ContainsKey(combination))
                {
                    evoGuns.Add(evoGunstMap[combination]);
                }
            }
        }
        return evoGuns;
    }
    private void EvolveWeapon(List<string> evoGuns, Player player)
    {
        List<Weapon> weaponInventory = player.GetWeapons();
        foreach (string evoGun in evoGuns)
        {
            Weapon existingWeapon = weaponInventory.Find(w => w.GetName() == evoGun);
            Debug.Log("Evool weapon");
           
            if (existingWeapon == null)
            {
                GameObject evoGunPrefab = Resources.Load<GameObject>("EvoGuns/"+evoGun);
                Weapon gunComponent = evoGunPrefab.GetComponent<Weapon>();
                player.Equip(gunComponent);
                Debug.Log("Ready evo");
            }
        }
    }
}
