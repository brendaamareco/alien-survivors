using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelUpSlot
{
    public UnityEngine.UIElements.Button button;
    public Weapon weapon;
    public Item item;
    public LevelUpController screen;

    public LevelUpSlot(Weapon weapon, VisualTreeAsset template, LevelUpController screen)
    {
        TemplateContainer itemButtonContainer = template.Instantiate();
        button = itemButtonContainer.Q<UnityEngine.UIElements.Button>();
        button.text = weapon.GetName();
        this.weapon = weapon;
        this.screen = screen;
        button.RegisterCallback<ClickEvent>(OnClickWeapon);
    }

    public LevelUpSlot(Item item, VisualTreeAsset template, LevelUpController screen)
    {
        TemplateContainer itemButtonContainer = template.Instantiate();
        button = itemButtonContainer.Q<UnityEngine.UIElements.Button>();
        button.text = item.GetName();
        this.item = item;
        this.screen = screen;
        button.RegisterCallback<ClickEvent>(OnClickItem);
    }

    public void OnClickWeapon(ClickEvent evt)
    {
        if (weapon != null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();
            // Get the player's weapon inventory
            List<Weapon> weaponInventory = player.GetWeapons();

            // Check if the player already has a weapon with the same name
            Weapon existingWeapon = weaponInventory.Find(w => w.GetName() == weapon.GetName());

            if (existingWeapon != null)
            {
                // Upgrade the existing weapon
                existingWeapon.Upgrade();
                Debug.Log("Upgraded " + existingWeapon.GetName());
            }
            else
            {
                // Equip the new weapon
                player.Equip(weapon);
                Debug.Log("Equipped " + weapon.GetName());
                GameEventManager.GetInstance().Publish(GameEvent.INVENTORY_CHANGED, new EventContext(this));
            }
        }
        screen.Hide();
    }

    public void OnClickItem(ClickEvent evt)
    {
        if (item != null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();
            // Get the player's item inventory
            List<Item> itemInventory = player.GetItems();

            // Check if the player already has an item with the same name
            Item existingItem = itemInventory.Find(i => i.GetName() == item.GetName());

            if (existingItem != null)
            {
                // Upgrade the existing item
                existingItem.Upgrade();
                Debug.Log("Upgraded " + existingItem.GetName());
            }
            else
            {
                // Equip the new item
                player.Equip(item);
                Debug.Log("Equipped " + item.GetName());
                GameEventManager.GetInstance().Publish(GameEvent.INVENTORY_CHANGED, new EventContext(this));
            }
        }
        screen.Hide();
    }
}
