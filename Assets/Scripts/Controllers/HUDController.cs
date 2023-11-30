using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] MenuPauseController pauseController;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject victory;
    [SerializeField] ExpController expController;

    private Player m_Player;
    private ProgressBar m_HealthBar;
    private ProgressBar m_ExperienceBar;
    private Label m_LevelLabel;
    private ProgressBar m_HealthBarBoss;

    private List<Weapon> m_WeaponInventory;
    private List<Item> m_ItemInventory;

    private void Start()
    {
        VisualElement container = root.rootVisualElement;
        Button btnPause = container.Q<Button>("BtnPause");
        btnPause.clicked += BtnPause_clicked;

        GameObject playerObject = GameObject.FindWithTag("Player");
        m_Player = playerObject.GetComponent<Player>();

        m_HealthBar = container.Q<ProgressBar>("HealthBar");
        m_HealthBar.highValue = m_Player.GetMaxHealthPoints();
        m_HealthBar.value = m_Player.GetCurrentHealthPoints();
        m_HealthBar.title = m_HealthBar.value + "/" + m_HealthBar.highValue;

        m_HealthBarBoss = container.Q<ProgressBar>("HealthBarBoss");
        m_HealthBarBoss.visible = false;
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, UpdateHealthBar);
        GameEventManager.GetInstance().Suscribe(GameEvent.HEALED, UpdateHealthBar);

        m_ExperienceBar = container.Q<ProgressBar>("ExperienceBar");
        m_ExperienceBar.highValue = expController.GetExpNeeded();
        m_ExperienceBar.value = m_Player.GetExperience();
        m_ExperienceBar.title = m_ExperienceBar.value + "/" + m_ExperienceBar.highValue;
        GameEventManager.GetInstance().Suscribe(GameEvent.EXPUP, UpdateExperienceBar);
        GameEventManager.GetInstance().Suscribe(GameEvent.GAME_OVER, PlayerIsDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.VICTORY, Victory);

        m_LevelLabel = container.Q<Label>("LevelLabel");
        m_LevelLabel.text = "Level 1";
        GameEventManager.GetInstance().Suscribe(GameEvent.LEVEL_UP, UpdateLevelLabel);

        SetWeaponInventory();
        SetItemInventory();
        GameEventManager.GetInstance().Suscribe(GameEvent.INVENTORY_CHANGED, SetInventorySprites);

    }

    private void SetInventorySprites(EventContext context)
    {
        SetWeaponInventory();
        SetItemInventory();
    }

    private void SetItemInventory()
    {
        m_ItemInventory = m_Player.GetItems();
        VisualElement container = root.rootVisualElement;
        int count = 1;
        foreach (Item item in m_ItemInventory)
        {
            VisualElement m_itemN = container.Q<VisualElement>("Item" + count);
            Texture2D backgroundImage = Resources.Load<Texture2D>("ItemSprite/" + item.GetName());
            StyleBackground styleBackground = new StyleBackground(backgroundImage);
            m_itemN.style.backgroundImage = styleBackground;

            Label m_itemNLabel = container.Q<Label>("Item" + count + "Label");
            m_itemNLabel.text = (item.GetLevel()).ToString();

            count += 1;
        }
    }

    private void SetWeaponInventory()
    {
        m_WeaponInventory = m_Player.GetWeapons();
        VisualElement container = root.rootVisualElement;
        int count = 1;
        foreach (Weapon weapon in m_WeaponInventory)
        {
            VisualElement m_weaponN = container.Q<VisualElement>("Weapon" + count);
            Texture2D backgroundImage = Resources.Load<Texture2D>("WeaponSprite/" + weapon.GetName());
            StyleBackground styleBackground = new StyleBackground(backgroundImage);
            m_weaponN.style.backgroundImage = styleBackground;

            Label m_weaponNLabel = container.Q<Label>("Weapon" + count + "Label");
            m_weaponNLabel.text = (weapon.GetLevel()).ToString();

            count += 1;
        }
    }

    private void BtnPause_clicked()
    {
        gameManager.SwitchPause();
        pauseController.Show();
    }

    private void UpdateHealthBar(EventContext context)
    {      
        m_HealthBar.highValue = m_Player.GetMaxHealthPoints();
        m_HealthBar.value = m_Player.GetCurrentHealthPoints();
        m_HealthBar.title = m_HealthBar.value + "/" + m_HealthBar.highValue;

        GameObject bossGo = GameObject.FindGameObjectWithTag("Boss");
        if (bossGo != null) 
        {
            Enemy boss = bossGo.GetComponent<Enemy>();

            m_HealthBarBoss.visible = true;
            m_HealthBarBoss.highValue = boss.GetMaxHealthPoints();
            m_HealthBarBoss.value = boss.GetCurrentHealthPoints();
            m_HealthBarBoss.title = m_HealthBarBoss.value + "/" + m_HealthBarBoss.highValue;
        }
    }

    private void UpdateExperienceBar(EventContext context)
    {
        m_ExperienceBar.highValue = expController.GetExpNeeded();
        m_ExperienceBar.value = m_Player.GetExperience() + 10;//Chequear esto
        m_ExperienceBar.title = m_ExperienceBar.value + "/" + m_ExperienceBar.highValue;
    }

    private void UpdateLevelLabel(EventContext context)
    {
        m_LevelLabel.text = "Level " + (m_Player.GetLevel()+1).ToString();
    }

    private void PlayerIsDead(EventContext context)
    {
        DeactivateHUD();
        gameOver.SetActive(true);
    }

    private void Victory(EventContext context)
    {
        DeactivateHUD();
        victory.SetActive(true);
    }

    private void DeactivateHUD() 
    {
        VisualElement container = root.rootVisualElement.Q<VisualElement>("Container");
        VisualElement content = root.rootVisualElement.Q<VisualElement>("Content");

        if (container != null)
            container.visible = false;

        if (content != null)  
            content.visible = false;
    }
}
