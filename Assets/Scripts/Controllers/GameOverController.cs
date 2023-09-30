using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;

    private string m_PantallaDerrotaPath = "Documents/MenuDerrota";
    private VisualElement m_PantallaDerrota;
    private VisualElement m_PopUpContainer;

    // Start is called before the first frame update
    void Start()
    {
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");

        VisualTreeAsset pantallaDerrotaAsset = Resources.Load<VisualTreeAsset>(m_PantallaDerrotaPath);
        m_PantallaDerrota = pantallaDerrotaAsset.Instantiate();
        m_PantallaDerrota.style.height = Length.Percent(100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show()
    {
        m_PopUpContainer.Add(m_PantallaDerrota);
        m_PopUpContainer.style.display = DisplayStyle.Flex;
    }
}
