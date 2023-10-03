using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] MenuPauseController pauseController;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject gameOver;
    [SerializeField] ExpController expController;

    private Player m_Player;
    private ProgressBar m_HealthBar;
    private ProgressBar m_ExperienceBar;

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
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, UpdateHealthBar);
        GameEventManager.GetInstance().Suscribe(GameEvent.HEALED, UpdateHealthBar);

        m_ExperienceBar = container.Q<ProgressBar>("ExperienceBar");
        m_ExperienceBar.highValue = expController.GetExpNeeded();
        m_ExperienceBar.value = m_Player.GetExperience();
        m_ExperienceBar.title = m_ExperienceBar.value + "/" + m_ExperienceBar.highValue;
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
        m_HealthBar.highValue = m_Player.GetMaxHealthPoints();
        m_HealthBar.value = m_Player.GetCurrentHealthPoints();
        m_HealthBar.title = m_HealthBar.value + "/" + m_HealthBar.highValue;
    }

    private void UpdateExperienceBar(EventContext context)
    {
        m_ExperienceBar.highValue = expController.GetExpNeeded();
        m_ExperienceBar.value = m_Player.GetExperience() + 10;//Chequear esto
        m_ExperienceBar.title = m_ExperienceBar.value + "/" + m_ExperienceBar.highValue;
    }

    private void PlayerIsDead(EventContext context)
    {
        gameManager.SwitchPause();
        DeactivateHUD();
        gameOver.SetActive(true);
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
