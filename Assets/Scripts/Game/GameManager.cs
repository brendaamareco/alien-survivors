using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UIObject;
    [SerializeField] GameState currentState;
    [SerializeField] float timeLimit;
    [SerializeField] GameObject boss1;
    [SerializeField] bool stopSpawn = false;
    [SerializeField] float spawnTime;

    private float stopwatchTime;
    private VisualElement rootStopwatch;

    private void Start()
    {
        Invoke("SpawnObject", spawnTime);
        ChangeState(GameState.Gameplay);
        
        GameEventManager.GetInstance().Suscribe(GameEvent.LEVEL_UP, HandleLevelUp);

        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        CinemachineVirtualCamera virtualCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = playerGo.transform;
        virtualCamera.LookAt = playerGo.transform;
    }

    private void HandleLevelUp(EventContext context)
    {
        SwitchLevelUp();
    }

    private void Update()
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

    private void ChangeState(GameState NewState)
    {
        currentState = NewState;
    }

    public void SwitchPause()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Gameplay);
            Time.timeScale = 1.0f;
        }
        else
        {
            ChangeState(GameState.Paused);
            Time.timeScale = 0;
        }
    }
    public void SwitchLevelUp()
    {
        if (currentState == GameState.LevelUp)
        {
            ChangeState(GameState.Gameplay);
            Time.timeScale = 1.0f;
        }
        else
        {
            ChangeState(GameState.LevelUp);
            Time.timeScale = 0;
        }
    }

    private void UpdateStopwacth()
    { 
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        CheckSpawnTime();

        if (stopwatchTime >= timeLimit)
        {
            //gameOver()
        }
    }
    
    private void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime/60);
        int seconds = Mathf.FloorToInt(stopwatchTime%60);

        rootStopwatch = UIObject.GetComponent<UIDocument>().rootVisualElement;

        Label stopwatch = rootStopwatch.Q<Label>("stopwatch");

        stopwatch.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CheckSpawnTime()
    {
        rootStopwatch = UIObject.GetComponent<UIDocument>().rootVisualElement;

        Label stopwatch = rootStopwatch.Q<Label>("stopwatch");

        //if (stopwatch.text == "00:10") {Debug.LogWarning("XXXXXXXXXXX");}
    }

    private void SpawnObject()
    {
        /*
         * Tuve que hacerlo de esta forma sino me tiraba error en la IA de ET:
         * 1) Arrastrar el prefab ET a la escena (Prefabs/Bosses/Et/Et) y desactivarlo para que no se vea
         * desde el editor (hay un checkbox)
         * 2) Activarlo con la línea de código de abajo:
         */
        boss1.SetActive(true);


        //Instantiate(boss1, transform.position, transform.rotation);
        /*if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }*/
    }
}
