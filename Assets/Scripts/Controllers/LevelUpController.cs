using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;

    private string m_LevelUpAssetPath = "Documents/LevelUp";
    private VisualElement m_LevelUp;
    private VisualElement m_PopUpContainer;
    public VisualTreeAsset itemBtnTemplate;

    public List<Weapon> weaponInventory;
    public List<Item> itemInventory;

    public string folderPath = "Assets/Prefabs/Items";
    //private List<GameObject> prefabs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");

        VisualTreeAsset levelUpAsset = Resources.Load<VisualTreeAsset>(m_LevelUpAssetPath);
        m_LevelUp = levelUpAsset.Instantiate();
        m_LevelUp.style.height = Length.Percent(100);

        GameObject playerObject = GameObject.FindWithTag("Player");
        Player player = playerObject.GetComponent<Player>();
        weaponInventory = player.GetWeapons();
        itemInventory = player.GetItems();

        // Randomly select one weapon
        Weapon selectedWeapon = weaponInventory.Count > 0 ? weaponInventory[Random.Range(0, weaponInventory.Count)] : null;

        // Randomly select one item
        Item selectedItem = itemInventory.Count > 0 ? itemInventory[Random.Range(0, itemInventory.Count)] : null;

        if (selectedWeapon != null)
        {
            LevelUpSlot invSlot = new LevelUpSlot(selectedWeapon, itemBtnTemplate, this);
            m_LevelUp.Q<VisualElement>("ItemContainer").Add(invSlot.button);
        }

        if (selectedItem != null)
        {
            LevelUpSlot invSlot = new LevelUpSlot(selectedItem, itemBtnTemplate, this);
            m_LevelUp.Q<VisualElement>("ItemContainer").Add(invSlot.button);
        }

        SelectRandomGun();
        SelectRandomItem();
    }
    void SelectRandomItem()
    {
        // Use Resources.LoadAll to load all item prefabs in the "Items" folder
        GameObject[] itemPrefabs = Resources.LoadAll<GameObject>("Items");

        // Create a list of item names that the player already has
        List<string> playerItemNames = new List<string>();

        foreach (Item item in itemInventory)
        {
            playerItemNames.Add(item.GetName());
        }

        // Create a list of item prefabs that the player doesn't have
        List<GameObject> availableItemPrefabs = new List<GameObject>();

        foreach (GameObject itemPrefab in itemPrefabs)
        {
            // Get the Item component from the prefab
            Item itemComponent = itemPrefab.GetComponent<Item>();

            // Check if the player doesn't have an item with the same name
            if (itemComponent != null && !playerItemNames.Contains(itemComponent.GetName()))
            {
                availableItemPrefabs.Add(itemPrefab);
            }
        }

        if (availableItemPrefabs.Count > 0)
        {
            // Select a random item prefab from the available list
            int randomIndex = Random.Range(0, availableItemPrefabs.Count);
            GameObject selectedPrefab = availableItemPrefabs[randomIndex];

            // Access the Item component inside the selected prefab
            Item itemComponent = selectedPrefab.GetComponent<Item>();

            if (itemComponent != null)
            {
                string itemName = itemComponent.GetName();
                Debug.Log("Selected Item Name: " + itemName);
                LevelUpSlot invSlot = new LevelUpSlot(itemComponent, itemBtnTemplate, this);
                m_LevelUp.Q<VisualElement>("ItemContainer").Add(invSlot.button);
            }
            else
            {
                Debug.LogWarning("Item component not found in the prefab.");
            }
        }
        else
        {
            Debug.LogWarning("No available item prefabs found in the 'Items' folder.");
        }
    }
    private void SelectRandomGun()
    {
        // Use Resources.LoadAll to load all gun prefabs in the "Guns" folder
        GameObject[] gunPrefabs = Resources.LoadAll<GameObject>("Guns");

        // Create a list of gun names that the player already has
        List<string> playerGunNames = new List<string>();

        foreach (Weapon weapon in weaponInventory)
        {
            playerGunNames.Add(weapon.GetName());
        }

        // Create a list of gun prefabs that the player doesn't have
        List<GameObject> availableGunPrefabs = new List<GameObject>();

        foreach (GameObject gunPrefab in gunPrefabs)
        {
            // Get the Weapon component from the prefab
            Weapon gunComponent = gunPrefab.GetComponent<Weapon>();

            // Check if the player doesn't have a gun with the same name
            if (gunComponent != null && !playerGunNames.Contains(gunComponent.GetName()))
            {
                availableGunPrefabs.Add(gunPrefab);
            }
        }

        if (availableGunPrefabs.Count > 0)
        {
            // Select a random gun prefab from the available list
            int randomIndex = Random.Range(0, availableGunPrefabs.Count);
            GameObject selectedPrefab = availableGunPrefabs[randomIndex];

            // Access the Weapon component inside the selected prefab
            Weapon gunComponent = selectedPrefab.GetComponent<Weapon>();

            if (gunComponent != null)
            {
                string gunName = gunComponent.GetName();
                Debug.Log("Selected Gun Name: " + gunName);
                LevelUpSlot invSlot = new LevelUpSlot(gunComponent, itemBtnTemplate, this);
                m_LevelUp.Q<VisualElement>("ItemContainer").Add(invSlot.button);
            }
            else
            {
                Debug.LogWarning("Gun component not found in the prefab.");
            }
        }
        else
        {
            Debug.LogWarning("No available gun prefabs found in the 'Guns' folder.");
        }
    }
    public void Show()
    {
        m_PopUpContainer.Add(m_LevelUp);
        m_PopUpContainer.style.display = DisplayStyle.Flex;
    }
    public void Hide()
    {
        m_PopUpContainer.Remove(m_LevelUp);
        m_PopUpContainer.style.display = DisplayStyle.None;
        gameManager.SwitchLevelUp();
    }
}
