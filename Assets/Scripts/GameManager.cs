using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Recorder.OutputPath;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnObject", spawnTime);
    }

    public enum GameState
    {
        Gameplay,
        Paused,
        LevelUp,
        GameOver
    }

    public GameState currentState;

    public float timeLimit;
    float stopwatchTime;

    public GameObject boss1;
    public bool stopSpawn = false;
    public float spawnTime;

    [SerializeField] private GameObject Stopwatch;
    private VisualElement rootStopwatch;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                UpdateStopwacth();
                break;
            case GameState.Paused: 
                break;
            case GameState.LevelUp:
                break;
            case GameState.GameOver:
                break;
            default:
                Debug.LogWarning("State does not exist");
                break;
        }
    }

    public void ChangeState(GameState NewState)
    {
        currentState = NewState;
    }

    public void UpdateStopwacth()
    { 
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        CheckSpawnTime();

        if (stopwatchTime >= timeLimit)
        {
            //gameOver()
        }
    }
    
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime/60);
        int seconds = Mathf.FloorToInt(stopwatchTime%60);

        rootStopwatch = Stopwatch.GetComponent<UIDocument>().rootVisualElement;

        Label stopwatch = rootStopwatch.Q<Label>("stopwatch");

        stopwatch.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckSpawnTime()
    {
        rootStopwatch = Stopwatch.GetComponent<UIDocument>().rootVisualElement;

        Label stopwatch = rootStopwatch.Q<Label>("stopwatch");

        //if (stopwatch.text == "00:10") {Debug.LogWarning("XXXXXXXXXXX");}
    }

    public void SpawnObject()
    {
        Debug.LogWarning("Bossu");
        Instantiate(boss1, transform.position, transform.rotation);
        /*if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }*/
    }
}
