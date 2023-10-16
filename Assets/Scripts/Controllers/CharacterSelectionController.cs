using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] PlayerFactory playerFactory;

    private PlayerId selectedPlayer = PlayerId.MICHI;
    private Label attackLbl;
    private Label speedLbl;
    private Label defenseLbl;
    private Label healthLbl;

    void Start()
    {
        VisualElement container = GetComponent<UIDocument>().rootVisualElement;

        container.Q<Button>("Personaje1").clicked += Michi_clicked;
        container.Q<Button>("Personaje2").clicked += Eastwood_clicked;
        container.Q<Button>("Personaje3").clicked += Fachero_clicked;
        container.Q<Button>("Personaje4").clicked += Detective_clicked;
        container.Q<Button>("BtnConfirm").clicked += CreatePlayer_clicked;

        attackLbl = container.Q<Label>("Ataque");
        defenseLbl = container.Q<Label>("Defensa");
        speedLbl = container.Q<Label>("Velocidad");
        healthLbl = container.Q<Label>("Vida");
    }

    private void CreatePlayer_clicked()
    {
        playerFactory.Create(selectedPlayer);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateStats()
    {
        Player player = playerFactory.GetPlayer(selectedPlayer);
        
        attackLbl.text = player.GetAttackPoints().ToString();
        defenseLbl.text = player.GetDefensePoints().ToString();
        speedLbl.text = player.GetSpeedPoints().ToString();
        healthLbl.text = player.GetMaxHealthPoints().ToString();
    }

    private void Detective_clicked()
    {
        selectedPlayer = PlayerId.DETECTIVE;
        UpdateStats();       
    }

    private void Fachero_clicked()
    {
        selectedPlayer = PlayerId.FACHERO;
        UpdateStats();
    }

    private void Eastwood_clicked()
    {
        selectedPlayer = PlayerId.EASTWOOD;
        UpdateStats();
    }

    private void Michi_clicked()
    {
        selectedPlayer = PlayerId.MICHI;
        UpdateStats();
    }
}
