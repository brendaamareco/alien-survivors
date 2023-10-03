using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] MenuPauseController pauseController;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject gameOver;

    [SerializeField] ExpController expController;

    private void Start()
    {
        VisualElement container = root.rootVisualElement;
        Button btnPause = container.Q<Button>("BtnPause");
        btnPause.clicked += BtnPause_clicked;

        GameObject playerObject = GameObject.FindWithTag("Player");
        Player player = playerObject.GetComponent<Player>();

        ProgressBar healthBar = container.Q<ProgressBar>("HealthBar");
        healthBar.highValue = player.GetMaxHealthPoints();
        healthBar.value = player.GetCurrentHealthPoints();
        healthBar.title = healthBar.value + "/" + healthBar.highValue;
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, UpdateHealthBar);

        ProgressBar experienceBar = container.Q<ProgressBar>("ExperienceBar");
        experienceBar.highValue = expController.GetExpNeeded();
        experienceBar.value = player.GetExperience();
        experienceBar.title = experienceBar.value + "/" + experienceBar.highValue;
        GameEventManager.GetInstance().Suscribe(GameEvent.EXPUP, UpdateExperienceBar);
        GameEventManager.GetInstance().Suscribe(GameEvent.GAME_OVER, PlayerIsDead);

    }

    private void BtnPause_clicked()
    {
        gameManager.SwitchPause();
        pauseController.Show();
    }

    private void UpdateHealthBar(EventContext context)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        Player player = playerObject.GetComponent<Player>();
        VisualElement container = root.rootVisualElement;
        ProgressBar healthBar = container.Q<ProgressBar>("HealthBar");
        healthBar.highValue = player.GetMaxHealthPoints();
        healthBar.value = player.GetCurrentHealthPoints();
        healthBar.title = healthBar.value + "/" + healthBar.highValue;
    }
    private void UpdateExperienceBar(EventContext context)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        Player player = playerObject.GetComponent<Player>();
        VisualElement container = root.rootVisualElement;
        ProgressBar experienceBar = container.Q<ProgressBar>("ExperienceBar");
        experienceBar.highValue = expController.GetExpNeeded();
        experienceBar.value = player.GetExperience()+10;//Chequear esto
        experienceBar.title = experienceBar.value + "/" + experienceBar.highValue;


    private void PlayerIsDead(EventContext context)
    {
        gameManager.SwitchPause();
        DeactivateHUD();
        gameOver.SetActive(true);

        //gameOverController.Show();
    }

    private void DeactivateHUD() {
        VisualElement container = root.rootVisualElement.Q<VisualElement>("Container");
        VisualElement content = root.rootVisualElement.Q<VisualElement>("Content");

        if (container != null) {
            container.visible = false;
        }
        if (content!= null)
        {
            content.visible = false;
        }

    }
}
