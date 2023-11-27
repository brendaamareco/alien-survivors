using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class ChestController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;

    private string m_ChestScreenAssetPath = "Documents/Chest";
    private VisualElement m_ChestScreen;
    private VisualElement m_PopUpContainer;
    private VisualElement chestImage;

    private List<Weapon> weaponInventory;
    private List<Item> itemInventory;

    private Label lblChest;
    private Button btnChest;

    private Player m_Player;

    public void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        m_Player = playerObject.GetComponent<Player>();

        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");

        VisualTreeAsset chestScreenAsset = Resources.Load<VisualTreeAsset>(m_ChestScreenAssetPath);
        m_ChestScreen = chestScreenAsset.Instantiate();
        m_ChestScreen.style.height = Length.Percent(100);

        chestImage = m_ChestScreen.Q<VisualElement>("chestImage");

        btnChest = m_ChestScreen.Q<Button>("chestButton");
        btnChest.clicked += BtnChest_clicked;

        lblChest = m_ChestScreen.Q<Label>("chestLabel");

        GameEventManager.GetInstance().Suscribe(GameEvent.CHEST_OPENED, HandleChestOpened);
    }

    private void HandleChestOpened(EventContext context)
    { Show(); }

    private void BtnChest_clicked()
    {
        int upgradeType = Random.Range(0, 2);
        SelectUpgrade(upgradeType);
        btnChest.clicked -= BtnChest_clicked;

        btnChest.text = "Close";
        btnChest.clicked += Hide;
    }
    private void SelectUpgrade(int upgradeType)
    {
        if (upgradeType == 0)
        {
            UpgradeItem();
        }
        else
        {
            UpgradeWeapon();
        }
        GameEventManager.GetInstance().Publish(GameEvent.INVENTORY_CHANGED, new EventContext(this));
    }

    //public void ChangeImageOpen()
    //{
    //    Texture2D backgroundImage = Resources.Load<Texture2D>("ChestSprite/cofreAbierto");
    //    StyleBackground styleBackground = new StyleBackground(backgroundImage);
    //    chestImage.style.backgroundImage = styleBackground;
    //}
    public void UpgradeItem()
    {
        // Upgrade an item
        itemInventory = m_Player.GetItems();
        if (itemInventory.Count > 0)
        {
            Item itemToUpgrade = itemInventory[Random.Range(0, itemInventory.Count)];
            itemToUpgrade.Upgrade();

            lblChest.text = itemToUpgrade.GetName()+" Upgraded!";

            Texture2D itemImage = Resources.Load<Texture2D>("ItemSprite/" + itemToUpgrade.GetName());
            StyleBackground itemBackground = new StyleBackground(itemImage);
            chestImage.style.backgroundImage = itemBackground;
        }
        else { UpgradeWeapon(); }

    }
    public void UpgradeWeapon()
    {
        // Upgrade a weapon
        weaponInventory = m_Player.GetWeapons();
        Weapon weaponToUpgrade = weaponInventory[Random.Range(0, weaponInventory.Count)];
        weaponToUpgrade.Upgrade();

        lblChest.text = weaponToUpgrade.GetName() + " Upgraded!";

        Texture2D weaponImage = Resources.Load<Texture2D>("WeaponSprite/" + weaponToUpgrade.GetName());
        StyleBackground weaponBackground = new StyleBackground(weaponImage);
        chestImage.style.backgroundImage = weaponBackground;
    }

    private void ResetScreen()
    {
        lblChest.text = "Chest Found!";
        btnChest.text = "Open";
        btnChest.clicked -= Hide;
        btnChest.clicked += BtnChest_clicked;
        Texture2D backgroundImage = Resources.Load<Texture2D>("ChestSprite/cofreCerrado");
        StyleBackground styleBackground = new StyleBackground(backgroundImage);
        chestImage.style.backgroundImage = styleBackground;
    }

    public void Show()
    {
        gameManager.SwitchLevelUp();
        m_PopUpContainer.Add(m_ChestScreen);
        m_PopUpContainer.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        m_PopUpContainer.Remove(m_ChestScreen);
        m_PopUpContainer.style.display = DisplayStyle.None;
        ResetScreen();
        gameManager.SwitchLevelUp();
    }
}
