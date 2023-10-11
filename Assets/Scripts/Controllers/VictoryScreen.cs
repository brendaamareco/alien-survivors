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
    private VisualElement m_PantallaVictoria;
    private VisualElement m_PopUpContainer;

    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;
    private PostProcessLayer m_PostProcessLayer;

    [SerializeField] Terrain terrain;
    [SerializeField] AudioSource audioVictory;
    private AudioSource audioTerrain;

    // Start is called before the first frame update
    void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");
        if (victoryScreen != null)
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

        audioTerrain = terrain.GetComponent<AudioSource>();
        if (audioTerrain != null && audioTerrain.isPlaying)
        {
            audioTerrain.Stop();
        }
        if (audioVictory != null)
        {
            audioVictory.Play();
        }
    }

    private void BtnContinue_clicked()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene(0);
    }
}
