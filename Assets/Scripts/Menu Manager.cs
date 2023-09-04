using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject MainMenu;
    private VisualElement rootMain;

    private void OnEnable()
    {
        // Main Menu Screen      
        rootMain = MainMenu.GetComponent<UIDocument>().rootVisualElement;

        Button buttonPlay = rootMain.Q<Button>("Start");

        buttonPlay.clicked += () => OnClickStart();
    }
    private void OnClickStart()
    {
        rootMain.style.display = DisplayStyle.None;
        SceneManager.LoadScene("SampleSceneAgustin");
        Debug.Log("Play Button");
    }
}
