using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] MenuPauseController pauseController;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        VisualElement container = root.rootVisualElement;
        Button btnPause = container.Q<Button>("BtnPause");
        btnPause.clicked += BtnPause_clicked;
    }

    private void BtnPause_clicked()
    {
        gameManager.SwitchPause();
        pauseController.Show();
    }
}
