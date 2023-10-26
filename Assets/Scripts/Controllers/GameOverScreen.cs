using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] VisualTreeAsset gameOverScreen;

    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;
    
    [SerializeField] AudioSource audioGameOver;
    
    private VisualElement m_PantallaDerrota;
    private VisualElement m_PopUpContainer;
    private PostProcessLayer m_PostProcessLayer;
    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameObject.FindObjectOfType<GameManager>(); 
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");
        
        if (gameOverScreen)
        {
            m_PantallaDerrota = gameOverScreen.Instantiate();
            m_PantallaDerrota.style.height = Length.Percent(100);
            m_PopUpContainer.Add(m_PantallaDerrota);
            m_PopUpContainer.style.display = DisplayStyle.Flex;
        }

        Button btnMainMenu = m_PantallaDerrota.Q<Button>("BtnMainMenu");
        btnMainMenu.clicked += BtnMainMenu_clicked;

        GameObject go = GameObject.FindGameObjectWithTag(postProcessLayerTag);
        m_PostProcessLayer = go.GetComponent<PostProcessLayer>();
        m_PostProcessLayer.volumeLayer = effectLayer;         
    }

    private void OnEnable()
    {
        GameObject backgroundMusicGo = GameObject.FindGameObjectWithTag("BackgroundMusic");
        
        if (backgroundMusicGo)
            backgroundMusicGo.SetActive(false);

        if (audioGameOver)
            audioGameOver.Play();
    }

    private void BtnMainMenu_clicked()
    { m_GameManager.GoToMainMenu(); }
}
