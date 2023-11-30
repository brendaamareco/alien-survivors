using System;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class ImitationTrainingEnv : MonoBehaviour
{
    [Header("Environment config")]
    [SerializeField] int maxEnvironmentSteps = 10000;
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] Transform loserPlace;
    [SerializeField] Material playerWinMaterial;
    [SerializeField] Material enemyWinMaterial;
    [SerializeField] Material tieMaterial;
    [SerializeField] MeshRenderer resultMeshRenderer;

    [Header("Global config")]
    [SerializeField] int agentsMinSpeed = 2;
    [SerializeField] int agentsMaxSpeed = 5;
    [SerializeField] int agentsMaxCooldown = 5;

    [Header("Player config")]
    [SerializeField] GameObject playerGo;
    [SerializeField] int playerMaxHP = 10;
    [SerializeField] int playerMaxAttack = 10;    

    [Header("Boss config")]
    [SerializeField] GameObject bossGo;
    [SerializeField] int bossMaxHP = 10;
    [SerializeField] int bossMaxAttack = 10;

    [Header("Enemies Range 1 config")]
    [SerializeField] GameObject enemiesRange1Parent;
    [SerializeField] int enemiesRange1MaxHP = 10;
    [SerializeField] int enemiesRange1MaxAttack = 10;

    [Header("Enemies Range 2 config")]
    [SerializeField] GameObject enemiesRange2Parent;
    [SerializeField] int enemiesRange2MaxHP = 10;
    [SerializeField] int enemiesRange2MaxAttack = 10;

    [Header("Enemies Range 3 config")]
    [SerializeField] GameObject enemiesRange3Parent;
    [SerializeField] int enemiesRange3MaxHP = 10;
    [SerializeField] int enemiesRange3MaxAttack = 10;

    private int m_ResetTimer = 0;
    private int m_DeadEnemies = 0;
    private GameObject m_SpawnAreas;
    private Agent playerAgent;
    private Agent bossAgent;

    private void Start()
    {
        m_SpawnAreas = GameObject.Find("SpawnAreas");

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleOnAttack);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);

        playerAgent = playerGo.GetComponent<Agent>();
        bossAgent = bossGo.GetComponent<Agent>();

        ResetScene();
    }

    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_DeadEnemies = 0;

        if (m_SpawnAreas != null)
            spawnArea = m_SpawnAreas.transform.GetChild(Random.Range(0, m_SpawnAreas.transform.childCount)).GetComponent<BoxCollider>();

        CreatePlayer();

        if (bossGo)
            SetUpBoss();

        if (enemiesRange1Parent.transform.childCount > 0)
            CreateEnemies(enemiesRange1Parent, enemiesRange1MaxAttack, enemiesRange1MaxHP);

        if (enemiesRange2Parent.transform.childCount > 0)
            CreateEnemies(enemiesRange2Parent, enemiesRange2MaxAttack, enemiesRange2MaxHP);

        if (enemiesRange3Parent.transform.childCount > 0)
            CreateEnemies(enemiesRange3Parent, enemiesRange3MaxAttack, enemiesRange3MaxHP);

    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;

        if (m_ResetTimer >= maxEnvironmentSteps && maxEnvironmentSteps > 0)
        {
            //Debug.Log("Tie");
            Player player = playerGo.GetComponent<Player>();

            bossAgent.AddReward(-2f * player.GetCurrentHealthPointsNormalized());
            bossAgent.EndEpisode();
            
            resultMeshRenderer.material = tieMaterial;
            ResetScene();
        }
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            DamageableEntity damageable = (DamageableEntity)context.GetEntity();
            Agent agent = damageable.gameObject.GetComponent<Agent>();

            if (agent && agent == playerAgent)
            {
                if (agent.CompareTag("Player"))
                {
                    //Debug.Log("Enemies team won");
                    bossAgent.AddReward(1f);
                    bossAgent.EndEpisode();

                    resultMeshRenderer.material = enemyWinMaterial;
                    ResetScene();
                }
            }

            else if (agent && agent == bossAgent)
            {
                agent.gameObject.transform.position = loserPlace.position;

                if (agent.CompareTag("Boss"))
                {
                    //Debug.Log("Player team won");

                    bossAgent.AddReward(-1f);
                    bossAgent.EndEpisode();

                    resultMeshRenderer.material = playerWinMaterial;
                    ResetScene();
                }    
            }
        }
        catch { } 
    }

    private void HandleOnAttack(EventContext context)
    {
        try
        {
            Weapon weapon = (Weapon)context.GetEntity();
            Agent agent = weapon.gameObject.GetComponentInParent<Agent>();

            if (agent && agent.CompareTag("Boss") && agent == bossAgent)
            {
                //Debug.Log("boss attacked");
                agent.AddReward(0.1f);
            }
        }
        catch { }
    }

    private void HandleOnDamage(EventContext context)
    {
        try
        {
            DamageableEntity damageable = (DamageableEntity)context.GetEntity();
            Agent agent = damageable.gameObject.GetComponent<Agent>();

            if (agent && agent == bossAgent)
            {
                bossAgent.AddReward(-0.01f);
            }

        }
        catch { }
    }

    private Enemy SetUpBoss()
    {
        bossGo.transform.localPosition = GetRandomPosition();

        Weapon weapon = bossGo.GetComponentInChildren<Weapon>();
        //weapon.SetCooldown(Random.Range(0.5f, agentsMaxCooldown));

        BaseStats bossStats = bossGo.GetComponent<BaseStats>();
        bossStats.SetDefense(0);
        //bossStats.SetAttack(Random.Range(10, bossMaxAttack + 1));
        bossStats.SetAttack(bossMaxAttack);
        //bossStats.SetHealth(Random.Range(10, bossMaxHP + 1));
	bossStats.SetHealth(bossMaxHP);
        //bossStats.SetSpeed(Random.Range(agentsMinSpeed, agentsMaxSpeed + 1));

        Enemy boss = bossGo.GetComponent<Enemy>();
        boss.ResetStats();
        boss.Heal(boss.GetMaxHealthPoints());

        return boss;        
    }

    private List<Enemy> CreateEnemies(GameObject enemiesParent, int enemiesMaxAttack, int enemiesMaxHP)
    {
        List<Enemy> enemies = new();
        int numberOfChilds = enemiesParent.transform.childCount;

        if (enemiesParent != null)
        {

            for (int i = 0; i < numberOfChilds; i++)
            {
                GameObject enemyGo = enemiesParent.transform.GetChild(i).gameObject;

                if (enemyGo.activeInHierarchy)
                {
                    //float yPos = enemyGo.transform.localPosition.y;
                    //Vector3 newPos = GetRandomPosition();
                    //newPos.y = (yPos - loserPlace.position.y < 0)? yPos : yPos - loserPlace.position.y;

                    enemyGo.transform.localPosition = GetRandomPosition();

                    enemyGo.GetComponent<DropSpawner>().enabled = false;
                    DamageableEntityRepresentation der = enemyGo.GetComponent<DamageableEntityRepresentation>();

                    if (der)
                    {
                        der.enabled = false;
                    }

                    Weapon weapon = enemyGo.GetComponentInChildren<Weapon>();
                    //weapon.SetCooldown(Random.Range(0.5f, agentsMaxCooldown));

                    BaseStats enemyStats = enemyGo.GetComponent<BaseStats>();
                    enemyStats.SetDefense(0);
		    enemyStats.SetAttack(enemiesMaxAttack);
                    //enemyStats.SetAttack(Random.Range(10, enemiesMaxAttack + 1));
                    enemyStats.SetHealth(enemiesMaxHP);
                    //enemyStats.SetSpeed(Random.Range(agentsMinSpeed, agentsMaxSpeed + 1));

                    Enemy enemy = enemyGo.GetComponent<Enemy>();
                    enemy.ResetStats();
                    enemy.Heal(enemy.GetMaxHealthPoints());

                    enemies.Add(enemy);
                }             
            }
        }      

        return enemies;

    }

    private Player CreatePlayer()
    {
        playerGo.transform.localPosition = GetRandomPosition();
        playerGo.GetComponent<PlayerController>().enabled = false;
        playerGo.GetComponent<DontDestroy>().enabled = false;
        playerGo.GetComponent<DamageableEntityRepresentation>().enabled = false;

        Weapon weapon = playerGo.GetComponentInChildren<Weapon>();
        //weapon.SetCooldown(Random.Range(0.5f, agentsMaxCooldown));

        BaseStats playerStats = playerGo.GetComponent<BaseStats>();
        playerStats.SetDefense(0);
        playerStats.SetAttack(playerMaxAttack);
        //playerStats.SetSpeed(Random.Range(1, 2));
        playerStats.SetHealth(playerMaxHP);

        Player player = playerGo.GetComponent<Player>();
        player.ResetStats();
        player.Heal(player.GetMaxHealthPoints());

        return player;
    }

    private Vector3 GetRandomPosition()
    {
        Bounds spawnerBounds = spawnArea.bounds;
        Vector3 globalRandomPosition = new Vector3(Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), spawnArea.transform.position.y,
                            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z));

        return transform.InverseTransformPoint(globalRandomPosition);
    }

}
