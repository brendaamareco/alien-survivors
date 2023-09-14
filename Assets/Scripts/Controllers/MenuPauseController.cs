using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuPauseController : MonoBehaviour
{
    [SerializeField] UIDocument root;
    [SerializeField] GameManager gameManager;

    private string m_MenuPauseAssetPath = "Documents/MenuPausa";
    private VisualElement m_MenuPause;
    private VisualElement m_PopUpContainer;

    public void Start()
    {       
        m_PopUpContainer = root.rootVisualElement.Q<VisualElement>("PopUp");
        
        VisualTreeAsset menuPauseAsset = Resources.Load<VisualTreeAsset>(m_MenuPauseAssetPath);
        m_MenuPause = menuPauseAsset.Instantiate();
        m_MenuPause.style.height = Length.Percent(100);

        Button btnContinue = m_MenuPause.Q<Button>("BtnContinue");
        btnContinue.clicked += BtnContinue_clicked;

        Button btnMainMenu = m_MenuPause.Q<Button>("BtnMainMenu");
        btnMainMenu.clicked += BtnMainMenu_clicked;

        Button btnExit = m_MenuPause.Q<Button>("BtnExit");
        btnExit.clicked += BtnExit_clicked;
    }

    private void BtnExit_clicked()
    {
        Application.Quit();
    }

    private void BtnMainMenu_clicked()
    {
        SceneManager.LoadScene(0);
    }

    private void BtnContinue_clicked()
    {
        gameManager.SwitchPause();
        Hide();
    }

    public void Show()
    {       
        m_PopUpContainer.Add(m_MenuPause);      
        m_PopUpContainer.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        m_PopUpContainer.Remove(m_MenuPause);
        m_PopUpContainer.style.display = DisplayStyle.None;
    }
}
