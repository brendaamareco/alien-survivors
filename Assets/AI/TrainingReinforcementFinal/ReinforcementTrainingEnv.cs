using System;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReinforcementTrainingEnv : MonoBehaviour
{
    [Header("Environment config")]
    [SerializeField] int maxEnvironmentSteps = 10000;
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] Transform loserPlace;
    [SerializeField] Material playerWinMaterial;
    [SerializeField] Material enemyWinMaterial;
    [SerializeField] Material tieMaterial;
    [SerializeField] MeshRenderer resultMeshRenderer;

    [Header("Player config")]
    [SerializeField] GameObject playerGo;
    [SerializeField] int playerMaxHP = 10;
    [SerializeField] int playerMaxAttack = 10;
    [SerializeField] int playerMaxSpeed = 5;

    [Header("Boss config")]
    [SerializeField] GameObject bossGo;
    [SerializeField] int bossMaxHP = 10;
    [SerializeField] int bossMaxAttack = 10;

    [Header("Enemies Range 1 config")]
    [SerializeField] GameObject enemiesRange1Parent;
    [SerializeField] int enemiesRange1MaxCant = 1;
    [SerializeField] int enemiesRange1MaxHP = 10;
    [SerializeField] int enemiesRange1MaxAttack = 10;

    [Header("Enemies Range 2 config")]
    [SerializeField] GameObject enemiesRange2Parent;
    [SerializeField] int enemiesRange2MaxCant = 1;
    [SerializeField] int enemiesRange2MaxHP = 10;
    [SerializeField] int enemiesRange2MaxAttack = 10;

    [Header("Enemies Range 3 config")]
    [SerializeField] GameObject enemiesRange3Parent;
    [SerializeField] int enemiesRange3MaxCant = 1;
    [SerializeField] int enemiesRange3MaxHP = 10;
    [SerializeField] int enemiesRange3MaxAttack = 10;

    private SimpleMultiAgentGroup m_GroupEnemy;
    private SimpleMultiAgentGroup m_GroupPlayer;
    private int m_ResetTimer = 0;
    private int m_DeadEnemies = 0;

    private Player m_Player;
    private Enemy m_Boss;
    private List<Enemy> m_EnemiesRange1;
    private List<Enemy> m_EnemiesRange2;
    private List<Enemy> m_EnemiesRange3;

    private void Start()
    {
        ResetScene();
    }

    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_DeadEnemies = 0;

        m_GroupEnemy = new SimpleMultiAgentGroup();
        m_GroupPlayer = new SimpleMultiAgentGroup();

        //DestroyAllObjects();

        m_Player = CreatePlayer();        

        m_Boss = CreateBoss();
        m_EnemiesRange1 = CreateEnemies(enemiesRange1Parent, enemiesRange1MaxCant, enemiesRange1MaxAttack, enemiesRange1MaxHP);
        m_EnemiesRange2 = CreateEnemies(enemiesRange2Parent, enemiesRange2MaxCant, enemiesRange2MaxAttack, enemiesRange2MaxHP);
        m_EnemiesRange3 = CreateEnemies(enemiesRange3Parent, enemiesRange3MaxCant, enemiesRange3MaxAttack, enemiesRange3MaxHP);   
    }

    private void DestroyAllObjects()
    {
        //if (m_Player != null)
        //    m_Player.gameObject.SetActive(false);

        //if (m_Boss != null)
        //    m_Boss.gameObject.SetActive(false);

        m_EnemiesRange1?.ForEach(x => x.gameObject.SetActive(false));
        m_EnemiesRange2?.ForEach(x => x.gameObject.SetActive(false));
        m_EnemiesRange3?.ForEach(x => x.gameObject.SetActive(false));
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;

        if (m_ResetTimer >= maxEnvironmentSteps && maxEnvironmentSteps > 0)
        {
            m_GroupEnemy.EndGroupEpisode();
            m_GroupPlayer.EndGroupEpisode();

            resultMeshRenderer.material = tieMaterial;
            ResetScene();
        }
    }


    private Enemy CreateBoss()
    {
        int bossCant = Random.Range(0, 2);

        if (bossCant > 0) 
        {
            bossGo.SetActive(true);
            bossGo.transform.localPosition = GetRandomPosition();

            Enemy boss = bossGo.GetComponent<Enemy>();
            boss.Heal(boss.GetMaxHealthPoints());

            BaseStats bossStats = bossGo.GetComponent<BaseStats>();
            bossStats.SetDefense(0);
            bossStats.SetAttack(Random.Range(10, bossMaxAttack + 1));
            bossStats.SetHealth(Random.Range(10, bossMaxHP + 1));

            m_GroupEnemy.RegisterAgent(bossGo.GetComponent<Agent>());

            return boss;
        }

        else
        {
            bossGo.SetActive(false);
            return null;
        }     
    }

    private List<Enemy> CreateEnemies(GameObject enemiesParent, int enemiesMaxCant, int enemiesMaxAttack, int enemiesMaxHP)
    {
        List<Enemy> enemies = new();
        int numberOfChilds = enemiesParent.transform.childCount;

        if (enemiesParent != null)
        {
            int numOfEnemies = Random.Range(1, Math.Min(numberOfChilds + 1, enemiesMaxCant + 1));

            for (int i = 0; i < numOfEnemies; i++)
            {

                GameObject enemyGo = enemiesParent.transform.GetChild(i).gameObject;
                enemyGo.SetActive(true);
                enemyGo.transform.localPosition = GetRandomPosition();
                enemyGo.GetComponent<DropSpawner>().enabled = false;
                enemyGo.GetComponent<DamageableEntityRepresentation>().enabled = false;

                Enemy enemy = enemyGo.GetComponent<Enemy>();
                enemy.Heal(enemy.GetMaxHealthPoints());
                
                BaseStats enemyStats = enemyGo.GetComponent<BaseStats>();
                enemyStats.SetDefense(0);
                enemyStats.SetAttack(Random.Range(10, enemiesMaxAttack + 1));
                enemyStats.SetHealth(Random.Range(10, enemiesMaxHP + 1));

                enemies.Add(enemy);
                m_GroupEnemy.RegisterAgent(enemyGo.GetComponent<Agent>());
            }

            for (int i = numOfEnemies; i < numberOfChilds; i++)
            {
                GameObject enemyGo = enemiesParent.transform.GetChild(i).gameObject;
                enemyGo.SetActive(false);
            }
        }      

        return enemies;

    }

    private Player CreatePlayer()
    {
        playerGo.GetComponent<PlayerController>().enabled = false;
        playerGo.GetComponent<DontDestroy>().enabled = false;

        Player player = playerGo.GetComponent<Player>();
        BaseStats playerStats = playerGo.GetComponent<BaseStats>();

        player.transform.localPosition = GetRandomPosition();
        player.Heal(player.GetMaxHealthPoints());

        playerStats.SetDefense(0);
        playerStats.SetAttack(Random.Range(10, playerMaxAttack+1));
        playerStats.SetSpeed(Random.Range(1, playerMaxSpeed + 1));
        playerStats.SetHealth(Random.Range(10, playerMaxHP + 1));

        m_GroupPlayer.RegisterAgent(playerGo.GetComponent<Agent>());

        return player;
    }

    private Vector3 GetRandomPosition()
    {
        Bounds spawnerBounds = spawnArea.bounds;
        Vector3 globalRandomPosition = new Vector3(Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
                            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z));

        return transform.InverseTransformPoint(globalRandomPosition);
    }

}


