using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Rendering.PostProcessing;

public class GameOverController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioSource audioGameOver;

    private string m_PantallaDerrotaPath = "Documents/MenuDerrota";
    private VisualElement m_PantallaDerrota;
    private VisualElement m_PopUpContainer;

    private AudioSource backgroundMusic;

    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;
    private PostProcessLayer m_PostProcessLayer;

    // Start is called before the first frame update
    void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");

        VisualTreeAsset pantallaDerrotaAsset = Resources.Load<VisualTreeAsset>(m_PantallaDerrotaPath);
        m_PantallaDerrota = pantallaDerrotaAsset.Instantiate();
        m_PantallaDerrota.style.height = Length.Percent(100);
        
        backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();

        GameObject go = GameObject.FindGameObjectWithTag(postProcessLayerTag);
        m_PostProcessLayer = go.GetComponent<PostProcessLayer>();
    }

    public void Show()
    {
        m_PostProcessLayer.volumeLayer = effectLayer;

        m_PopUpContainer.Add(m_PantallaDerrota);
        m_PopUpContainer.style.display = DisplayStyle.Flex;

        if (backgroundMusic && backgroundMusic.isPlaying)
            backgroundMusic.Stop();

        if (audioGameOver)
            audioGameOver.Play();
    }
}
