using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] AudioSource btnClickSfx;

    PlayerFactory playerFactory;
    private string selectedPlayer = "Michi";
    private Label attackLbl;
    private Label speedLbl;
    private Label defenseLbl;
    private Label healthLbl;

    private GameObject playerSelected;

    private void Awake()
    {
        playerFactory = GameObject.FindAnyObjectByType<PlayerFactory>();
    }

    void Start()
    {
        VisualElement container = GetComponent<UIDocument>().rootVisualElement;

        container.Q<Button>("Personaje1").Focus();
        container.Q<Button>("Personaje1").clicked += Michi_clicked;
        container.Q<Button>("Personaje2").clicked += Eastwood_clicked;
        container.Q<Button>("Personaje3").clicked += Detective_clicked;
        container.Q<Button>("Personaje4").clicked += Fachero_clicked;

        container.Q<Button>("BtnConfirm").clicked += CreatePlayer_clicked;

        attackLbl = container.Q<Label>("Ataque");
        defenseLbl = container.Q<Label>("Defensa");
        speedLbl = container.Q<Label>("Velocidad");
        healthLbl = container.Q<Label>("Vida");

        Time.timeScale = 1.0f;
        UpdateStats();
    }

    private void CreatePlayer_clicked()
    {
        btnClickSfx.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void UpdateStats()
    {
        playerSelected = playerFactory.Create(selectedPlayer);

        Player player = playerSelected.GetComponent<Player>();
        
        attackLbl.text = "Ataque: " + player.GetAttackPoints().ToString();
        defenseLbl.text = "Defensa: " + player.GetDefensePoints().ToString();
        speedLbl.text = "Velocidad: " + player.GetSpeedPoints().ToString();
        healthLbl.text = "Vida: " + player.GetMaxHealthPoints().ToString();
    }

    private void Detective_clicked()
    {
        btnClickSfx.Play();

        selectedPlayer = "MichiDetective";
        UpdateStats();       
    }

    private void Fachero_clicked()
    {
        btnClickSfx.Play();

        selectedPlayer = "MichiFachero";
        UpdateStats();
    }

    private void Eastwood_clicked()
    {
        btnClickSfx.Play();

        selectedPlayer = "MichiEastwood";
        UpdateStats();
    }

    private void Michi_clicked()
    {
        btnClickSfx.Play();

        selectedPlayer = "Michi";
        UpdateStats();
    }
}
