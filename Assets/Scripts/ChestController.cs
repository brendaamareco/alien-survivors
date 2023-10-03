using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChestController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;

    private string m_ChestScreenAssetPath = "Documents/Chest";
    private VisualElement m_ChestScreen;
    private VisualElement m_PopUpContainer;

    private List<Weapon> weaponInventory;
    private List<Item> itemInventory;

    public void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");

        VisualTreeAsset chestScreenAsset = Resources.Load<VisualTreeAsset>(m_ChestScreenAssetPath);
        m_ChestScreen = chestScreenAsset.Instantiate();
        m_ChestScreen.style.height = Length.Percent(100);

        Button btnChest = m_ChestScreen.Q<Button>("chestButton");
        btnChest.clicked += BtnChest_clicked;

        GameEventManager.GetInstance().Suscribe(GameEvent.CHEST_OPENED, HandleChestOpened);
    }

    private void HandleChestOpened(EventContext context)
    { Show(); }

    private void BtnChest_clicked()
    {
        Debug.Log("XXXXXXXXXXXXXXX");
        gameManager.SwitchLevelUp();
        GameObject playerObject = GameObject.FindWithTag("Player");
        Player player = playerObject.GetComponent<Player>();

        // Randomly select whether to upgrade an item or a weapon.
        int upgradeType = Random.Range(0, 2);
        // Get the corresponding inventory from the player.
        if (upgradeType == 0)
        {
            itemInventory = player.GetItems();
            Item itemToUpgrade = itemInventory[Random.Range(0, itemInventory.Count)];
            itemToUpgrade.Upgrade();
        }
        else
        {
            weaponInventory = player.GetWeapons();
            Weapon weaponToUpgrade = weaponInventory[Random.Range(0, weaponInventory.Count)];
            weaponToUpgrade.Upgrade();
        }
        Hide();
    }

    public void Show()
    {
        m_PopUpContainer.Add(m_ChestScreen);
        m_PopUpContainer.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        m_PopUpContainer.Remove(m_ChestScreen);
        m_PopUpContainer.style.display = DisplayStyle.None;
    }
}
