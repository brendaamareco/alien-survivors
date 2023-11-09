using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] VisualTreeAsset victoryScreen;    

    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;
    
    [SerializeField] AudioSource audioVictory;

    private VisualElement m_PantallaVictoria;
    private VisualElement m_PopUpContainer;
    private PostProcessLayer m_PostProcessLayer;
    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameObject.FindObjectOfType<GameManager>();

        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");
        if (victoryScreen)
        {
            m_PantallaVictoria = victoryScreen.Instantiate();
            m_PantallaVictoria.style.height = Length.Percent(100);
            m_PopUpContainer.Add(m_PantallaVictoria);
            m_PopUpContainer.style.display = DisplayStyle.Flex;
        }

        Button btnContinue = m_PantallaVictoria.Q<Button>("BtnContinue");
        btnContinue.clicked += BtnContinue_clicked;

        GameObject go = GameObject.FindGameObjectWithTag(postProcessLayerTag);
        m_PostProcessLayer = go.GetComponent<PostProcessLayer>();
        m_PostProcessLayer.volumeLayer = effectLayer;
    }

    private void OnEnable()
    {
        GameObject backgroundMusicGo = GameObject.FindGameObjectWithTag("BackgroundMusic");

        if (backgroundMusicGo)
            backgroundMusicGo.SetActive(false);

        if (audioVictory)
            audioVictory.Play();
    }

    private void BtnContinue_clicked()
    { 
        m_GameManager.NextLevel();
    }
}
