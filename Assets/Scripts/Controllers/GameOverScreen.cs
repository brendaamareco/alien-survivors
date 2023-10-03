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
    private VisualElement m_PantallaDerrota;
    private VisualElement m_PopUpContainer;

    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;
    private PostProcessLayer m_PostProcessLayer;

    [SerializeField] Terrain terrain;
    [SerializeField] AudioSource audioGameOver;
    private AudioSource audioTerrain;

    // Start is called before the first frame update
    void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");
        if (gameOverScreen != null)
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

        audioTerrain = terrain.GetComponent<AudioSource>();
        if (audioTerrain != null && audioTerrain.isPlaying)
        {
            audioTerrain.Stop();
        }
        if (audioGameOver != null)
        {
            audioGameOver.Play();
        }
    }

    private void BtnMainMenu_clicked()
    {
        SceneManager.LoadScene(0);
    }
}
