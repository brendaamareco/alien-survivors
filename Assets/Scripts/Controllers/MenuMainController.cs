using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    private VisualElement m_RootMain;

    private void OnEnable()
    {
        // Main Menu Screen      
        m_RootMain = mainMenu.GetComponent<UIDocument>().rootVisualElement;

        Button buttonPlay = m_RootMain.Q<Button>("Start");

        buttonPlay.clicked += () => OnClickStart();
    }
    private void OnClickStart()
    {
        m_RootMain.style.display = DisplayStyle.None;
        SceneManager.LoadScene("Level1");
    }
}
