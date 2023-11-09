using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UIObject;
    [SerializeField] GameState currentState;
    [SerializeField] float timeLimit;
    [SerializeField] float bossSpawnTime = 60.0f;

    private float stopwatchTime;
    private VisualElement rootStopwatch;

    //SPAWNER
    [SerializeField] GameObject[] rank1Enemies;
    [SerializeField] GameObject[] rank2Enemies;
    [SerializeField] GameObject[] rank3Enemies;
    [SerializeField] GameObject boss;

    private float spawnTime = 0.5f;    // Initial spawn time
    private float timer = 0.0f;
    private bool bossDefeated = false;
    private float spawnMediumTime; 
    private float spawnFinalTime;
    private Player player;
    private bool bossSpawned = false;

    private void Start()
    {
        Time.timeScale = 1.0f;
        spawnMediumTime = bossSpawnTime / 2f;
        spawnFinalTime = spawnMediumTime + bossSpawnTime / 4f;

        GameEventManager.GetInstance().Suscribe(GameEvent.LEVEL_UP, HandleLevelUp);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        CinemachineVirtualCamera virtualCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;

        GameEventManager.GetInstance().Suscribe(GameEvent.GAME_OVER, PlayerIsDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.VICTORY, Victory);

        //Invoke("SpawnBoss", bossSpawnTime);
        StartCoroutine(SpawnEnemies());
    }

    private void Victory(EventContext obj)
    {
        BossDefeated();
        SwitchPause();
        //GameEventManager.GetInstance().Reset();
    }

    private void PlayerIsDead(EventContext obj)
    {
        SwitchPause();
        //GameEventManager.GetInstance().Reset();
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

        //if (stopwatchTime >= timeLimit)
        //{
        //    GameEventManager.GetInstance().Publish(GameEvent.GAME_OVER, new EventContext(this));
        //}
        
        if (stopwatchTime >= bossSpawnTime)
            SpawnBoss();
    }

    public void GoToMainMenu()
    {
        GameEventManager.GetInstance().Reset();
        SceneManager.LoadScene(0);
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
    }

    private void SpawnBoss()
    {
        if (!bossSpawned)
        {
            Vector3 bossPosition = new Vector3(player.transform.position.x, 10f, player.transform.position.z);
            Instantiate(boss, bossPosition, Quaternion.identity);
            bossSpawned = true;
        }        
    }

    private void SpawnEnemy(GameObject[] enemies, float probPerseguidor, float probLejano, float probEstatico)
    {
        if (enemies.Length > 0)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();
            float probability = Random.value;
            int enemyIndex = -1;           

            if (probability <= probPerseguidor)
                enemyIndex = 0;

            else if (probability <= probPerseguidor + probLejano)
                enemyIndex = 1;

            else if (probability <= probPerseguidor + probLejano + probEstatico)
                enemyIndex = 2;
          
            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = 0; // Ensure enemies spawn at the same ground level
            Vector3 spawnPosition = player.transform.position + randomDirection * 15f; // Adjust the radius as needed
            RaycastHit hit;

            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("GroundLayer")))
                spawnPosition = hit.point;

            Instantiate(enemies[enemyIndex], spawnPosition, Quaternion.identity).SetActive(true);
        }
    }


    private void SpawnRankedEnemies(float probRank1, float probRank2, float probRank3)
    {
        float probPerseguidor = 0.6f;
        float probLejano = 0.35f;
        float probEstatico = 0.05f;
        float probabilityRandom = Random.value;

        if (probabilityRandom <= probRank1)
            SpawnEnemy(rank1Enemies, probPerseguidor, probLejano, probEstatico);

        else if (probabilityRandom <= probRank1 + probRank2)
            SpawnEnemy(rank2Enemies, probPerseguidor, probLejano, probEstatico);

        else if (probabilityRandom <= probRank1 + probRank2 + probRank3)
            SpawnEnemy(rank3Enemies, probPerseguidor, probLejano, probEstatico);
    }

    private IEnumerator SpawnEnemies()
    {
        while (!bossDefeated)
        {
            yield return new WaitForSeconds(spawnTime);

            timer += spawnTime;

            if (timer > 0 && timer < spawnMediumTime)
                SpawnRankedEnemies(0.9f, 0.1f, 0.0f);

            if (timer >= spawnMediumTime && timer < spawnFinalTime)
                SpawnRankedEnemies(0.6f, 0.35f, 0.05f);

            if (timer >= spawnFinalTime)
                SpawnRankedEnemies(0.1f, 0.6f, 0.3f);
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;
        StopAllCoroutines();
    }
}
