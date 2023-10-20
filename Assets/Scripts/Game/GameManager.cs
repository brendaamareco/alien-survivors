using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UIObject;
    [SerializeField] GameState currentState;
    [SerializeField] float timeLimit;

    private float stopwatchTime;
    private VisualElement rootStopwatch;

    ///

    [SerializeField] GameObject[] rank1Enemies;
    [SerializeField] GameObject[] rank2Enemies;
    [SerializeField] GameObject[] rank3Enemies;
    [SerializeField] GameObject boss;

    private float spawnTime = 2.0f;    // Initial spawn time
    private float timer = 0.0f;
    private float bossSpawnTime = 60.0f;
    //private float bossSpawnTime = 300.0f;
    private bool bossDefeated = false;
    ///

    private void Start()
    {
        Invoke("SpawnBoss", bossSpawnTime);
        StartCoroutine(SpawnEnemies());
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

    private void SpawnBoss()
    {
        /*
         * Tuve que hacerlo de esta forma sino me tiraba error en la IA de ET:
         * 1) Arrastrar el prefab ET a la escena (Prefabs/Bosses/Et/Et) y desactivarlo para que no se vea
         * desde el editor (hay un checkbox)
         * 2) Activarlo con la línea de código de abajo:
         */
        boss.SetActive(true);


        //Instantiate(boss1, transform.position, transform.rotation);
        /*if (stopSpawn)
        {
            CancelInvoke("SpawnObject");
        }*/
    }
    //////////////////////////////////////////////////////////////////////

    private void SpawnObject(GameObject[] enemies)
    {
        if (enemies.Length > 0)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();

            int randomIndex = Random.Range(0, enemies.Length);
            //Vector3 spawnPosition = player.transform.position + (Random.insideUnitSphere * 5f); // Adjust the radius as needed
            //Instantiate(enemies[randomIndex], spawnPosition, Quaternion.identity).SetActive(true);

            Vector3 randomDirection = Random.onUnitSphere;
            randomDirection.y = 0; // Ensure enemies spawn at the same ground level
            Vector3 spawnPosition = player.transform.position + randomDirection * 15f; // Adjust the radius as needed
            RaycastHit hit;

            if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("GroundLayer")))
            {
                spawnPosition = hit.point;
            }

            Instantiate(enemies[randomIndex], spawnPosition, Quaternion.identity).SetActive(true);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (!bossDefeated)
        {
            /*
            GameObject[] rank1EnemiesClone = new GameObject[rank1Enemies.Length];
            for (int i = 0; i < rank1Enemies.Length; i++)
            {
                GameObject clonedGameObject = Instantiate(rank1Enemies[i]);
                rank1EnemiesClone[i] = clonedGameObject;
            }
            */
            yield return new WaitForSeconds(spawnTime);
            SpawnObject(rank1Enemies);

            timer += spawnTime;

            if (timer >= 10.0f)
            {
                // After 2 minutes, spawn rank 2 enemies
                spawnTime = 4.0f;
                SpawnObject(rank2Enemies);
            }

            if (timer >= 15.0f)
            {
                // After 4 minutes, spawn rank 3 enemies
                spawnTime = 6.0f;
                SpawnObject(rank2Enemies);
                SpawnObject(rank3Enemies);
            }
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;
        // Stop spawning enemies
        StopAllCoroutines();
    }

    //////////////////////////////////////////////////////////////////////

}
