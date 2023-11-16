using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UIObject;
    [SerializeField] GameState currentState;
    [SerializeField] float timeLimit;

    [Header("Boss Spawner")]
    [SerializeField] float bossSpawnTime = 60.0f;
    [SerializeField] GameObject boss;

    [Header("Enemies Spawner")]
    [SerializeField] float spawnTime = 1.5f;
    [SerializeField] float probPerseguidor = 0.6f;
    [SerializeField] float probLejano = 0.35f;
    [SerializeField] float probEstatico = 0.05f;
    [SerializeField] GameObject[] rank1Enemies;
    [SerializeField] GameObject[] rank2Enemies;
    [SerializeField] GameObject[] rank3Enemies;

    private float timer = 0.0f;
    private bool bossDefeated = false;
    private float spawnMediumTime; 
    private float spawnFinalTime;
    private Player player;
    private bool bossSpawned = false;
    private float stopwatchTime;
    private VisualElement rootStopwatch;

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
        GameEventManager.GetInstance().Suscribe(GameEvent.FINISH_LEVEL, FinishLevel);
        //GameEventManager.GetInstance().Suscribe(GameEvent.VICTORY, Victory);

        //Invoke("SpawnBoss", bossSpawnTime);
        StartCoroutine(SpawnEnemies());
    }

    private void FinishLevel(EventContext obj) 
    {
        Victory(obj);
    }

    private void Victory(EventContext obj)
    {
        BossDefeated();
        SwitchPause();
        GameEventManager.GetInstance().Publish(GameEvent.VICTORY, obj);
    }

    private void PlayerIsDead(EventContext obj)
    {
        SwitchPause();
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

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            player.Heal(player.GetMaxHealthPoints());
            GameEventManager.GetInstance().Reset();
            SceneManager.LoadScene(currentIndex+1);

            DamageableEntityRepresentation playerRepresentation = player.gameObject.GetComponent<DamageableEntityRepresentation>();
            playerRepresentation.Reset();
        }
        
        else
            GoToMainMenu();
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
