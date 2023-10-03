using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    public AudioSource startButtonAudioSource;

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
        if (startButtonAudioSource != null)
        {
            // Reproduce la pista de audio "StartButtonMusic"
            startButtonAudioSource.Play();
        }
        else
        {
            Debug.LogError("No se encontró el componente startButtonAudioSource.");
        }

        SceneManager.LoadScene("Level1");
    }
}
