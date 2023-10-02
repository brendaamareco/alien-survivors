using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] MenuPauseController pauseController;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject gameOver;

    private void Start()
    {
        VisualElement container = root.rootVisualElement;
        Button btnPause = container.Q<Button>("BtnPause");
        btnPause.clicked += BtnPause_clicked;

        GameEventManager.GetInstance().Suscribe(GameEvent.GAME_OVER, PlayerIsDead);
    }

    private void BtnPause_clicked()
    {
        gameManager.SwitchPause();
        pauseController.Show();
    }

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
