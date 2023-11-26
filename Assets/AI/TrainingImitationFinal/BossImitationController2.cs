using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class BossImitationController2 : MonoBehaviour
{
    private Player m_player;
    private Enemy m_boss;
    private Agent m_agent;
    private List<Enemy> enemies;

    [Header("Environment config")]
    [SerializeField] int maxEnvironmentSteps = 10000;
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] Transform loserPlace;
    [SerializeField] Material playerWinMaterial;
    [SerializeField] Material enemyWinMaterial;
    [SerializeField] MeshRenderer resultMeshRenderer;

    [Header("Global config")]
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

    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= maxEnvironmentSteps && maxEnvironmentSteps > 0)
        {
            resultMeshRenderer.material = enemyWinMaterial;
            m_agent.AddReward(-1f);
            m_agent.EndEpisode();
            ResetScene();
        }
    }

    private bool EtIsCovered(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
    {
        bool isCloseToAnAlien = EtIsCloseToAnAlien(etPosition, alienPosition);
        bool isAlienBetweenEtAndPlayer = AlienIsBetweenEtAndPlayer(etPosition, alienPosition, catPosition);
        bool isEtAligned = IsEtAligned(etPosition, alienPosition, catPosition);


        if (isCloseToAnAlien && isEtAligned && isAlienBetweenEtAndPlayer)
        {
            return true;
        }
        return false;
    }

    private bool AlienIsBetweenEtAndPlayer(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
    {
        float distanceOnEjeXBetweenEtAndAlien = Mathf.Abs(etPosition.x - alienPosition.x);
        float distanceOnEjeZBetweenEtAndAlien = Mathf.Abs(etPosition.z - alienPosition.z);
        float distanceOnEjeXBetweenEtAndPlayer = Mathf.Abs(etPosition.x - catPosition.x);
        float distanceOnEjeZBetweenEtAndPlayer = Mathf.Abs(etPosition.z - catPosition.z);
        float distanceOnEjeXBetweenAlienAndPlayer = Mathf.Abs(alienPosition.x - catPosition.x);
        float distanceOnEjeZBetweenAlienAndPlayer = Mathf.Abs(alienPosition.z - catPosition.z);

        bool alienIsCloserToPlayerOnEjeX = distanceOnEjeXBetweenAlienAndPlayer < distanceOnEjeXBetweenEtAndPlayer;
        bool alienIsCloserToPlayerOnEjeZ = distanceOnEjeZBetweenAlienAndPlayer < distanceOnEjeZBetweenEtAndPlayer;

        bool alienIsInTheMiddleOnEjeX = distanceOnEjeXBetweenEtAndAlien < distanceOnEjeXBetweenEtAndPlayer;
        bool alienIsInTheMiddleOnEjeZ = distanceOnEjeZBetweenEtAndAlien < distanceOnEjeZBetweenEtAndPlayer;

        if (alienIsCloserToPlayerOnEjeX && alienIsCloserToPlayerOnEjeZ && alienIsInTheMiddleOnEjeX && alienIsInTheMiddleOnEjeZ)
        {
            return true;
        }
        return false;
    }

    private bool EtIsCloseToAnAlien(Vector3 etPosition, Vector3 alienPosition) 
    {
        float distanciaMaxima = 4.0f;
        float distanceOnEjeX = Mathf.Abs(etPosition.x - alienPosition.x);
        float distanceOnEjeZ = Mathf.Abs(etPosition.z - alienPosition.z);

        //Debug.Log($"Distancia en eje x: {distanceOnEjeX}");
        //Debug.Log($"Distancia en eje z: {distanceOnEjeZ}");

        bool isCloseOnEjeX = distanceOnEjeX < distanciaMaxima;
        bool isCloseOnEjeZ = distanceOnEjeZ < distanciaMaxima;

        if (isCloseOnEjeX && isCloseOnEjeZ) 
        {
            return true;
        }
        return false;
    }

    private bool IsEtAligned(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
    {
        float angle = GetEtAngle(etPosition, alienPosition, catPosition);

        // Comprueba si ET está detrás del alien
        if (angle > 155f)
        {
            return true;
        }
        return false;
    }

    private float GetEtAngle(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
    {
        // Calcula el vector que va desde el alien al gato
        Vector3 vectorAlienToGato = catPosition - alienPosition;

        // Calcula el vector que va desde el alien a ET
        Vector3 vectorAlienToET = etPosition - alienPosition;

        // Calcula el ángulo entre estos dos vectores
        float angle = Vector3.Angle(vectorAlienToGato, vectorAlienToET);

        return angle;
    }
    private void HandleOnDead(EventContext context)
    {
        try
        {
            DamageableEntity damageable = (DamageableEntity)context.GetEntity();
            Agent agent = damageable.gameObject.GetComponent<Agent>();

            Enemy enemy = damageable.gameObject.GetComponent<Enemy>();

            if (agent && agent == m_player.GetComponent<Agent>())
            {
                if (agent.CompareTag("Player"))
                {
                    Debug.Log("Player murio**");
                    m_agent.AddReward(+1f);
                    m_agent.EndEpisode();
                    ResetScene();
                }
            }
            else if (agent && agent == m_boss.GetComponent<Agent>())
            {
                if (agent.CompareTag("Boss"))
                {
                    Debug.Log("Et murio**");
                    resultMeshRenderer.material = enemyWinMaterial;
                    //m_agent.AddReward(-1f);
                    m_agent.EndEpisode();
                    ResetScene();
                }
            }
            else if (agent && enemies.Contains(enemy))
            {
                Debug.Log("Enemy murio**");
                m_DeadEnemies += 1;
                enemy.gameObject.transform.position = loserPlace.position;
                if (m_DeadEnemies == enemies.Count)
                {
                    m_agent.EndEpisode();
                    ResetScene();
                }
            }
            else {
                Debug.Log("No encontre nada");
            }
        }
        catch { }
    }
    /*
    private void HandleOnDead(EventContext context)
    {
        DamageableEntity damageable = (DamageableEntity)context.GetEntity();
        Enemy enemy = damageable.gameObject.GetComponent<Enemy>();

        if (enemy == m_boss)
        {
            if (enemy.CompareTag("Boss")) { 
                Debug.Log("Et murio**");
                resultMeshRenderer.material = enemyWinMaterial;
                m_agent.AddReward(-1f);
                m_agent.EndEpisode();
                ResetScene();
            }
        }
        else if ((Player)context.GetEntity() == m_player)
        {
            Debug.Log("Player murio**");
            m_agent.EndEpisode();
            ResetScene();
        }
        else if(enemies.Contains(enemy))
        {
            Debug.Log("Enemy murio**");
            m_DeadEnemies += 1;
            enemy.gameObject.transform.position = loserPlace.position;
            if (m_DeadEnemies == enemies.Count)
            {
                m_agent.EndEpisode();
                ResetScene();
            }            
        }
    }
    */

    private void HandleOnDamage(EventContext context)
    {
        if ((DamageableEntity)context.GetEntity() == m_boss)
        {
            m_agent.AddReward(-0.01f);
        }
    }

    private void ResetScene()
    {
        enemies = new List<Enemy>();
        m_ResetTimer = 0;
        m_DeadEnemies = 0;

        m_player = CreatePlayer();
        m_boss = SetUpBoss();

        m_agent = m_boss.transform.GetComponent<Agent>();
        if (m_agent)
        {
            Debug.Log("Agent encontrado");
        }
        else
        {
            Debug.Log("Agent no encontrado");
        }

        List<Enemy> enemiesRange1 = CreateEnemies(enemiesRange1Parent, enemiesRange1MaxAttack, enemiesRange1MaxHP);
        List<Enemy> enemiesRange2 = CreateEnemies(enemiesRange2Parent, enemiesRange2MaxAttack, enemiesRange2MaxHP);
        List<Enemy> enemiesRange3 = CreateEnemies(enemiesRange3Parent, enemiesRange3MaxAttack, enemiesRange3MaxHP);

        enemies.AddRange(enemiesRange1);
        enemies.AddRange(enemiesRange2);
        enemies.AddRange(enemiesRange3);
    }
    private Enemy SetUpBoss()
    {
        bossGo.transform.localPosition = GetRandomPosition();

        Weapon weapon = bossGo.GetComponentInChildren<Weapon>();
        //weapon.SetCooldown(Random.Range(0.5f, agentsMaxCooldown));

        BaseStats bossStats = bossGo.GetComponent<BaseStats>();
        bossStats.SetDefense(0);
        bossStats.SetAttack(Random.Range(10, bossMaxAttack + 1));
        bossStats.SetHealth(Random.Range(10, bossMaxHP + 1));
        //bossStats.SetSpeed(Random.Range(2, agentsMaxSpeed + 1));

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
                    enemyGo.transform.localPosition = GetRandomPosition();
                    enemyGo.GetComponent<DropSpawner>().enabled = false;
                    enemyGo.GetComponent<DamageableEntityRepresentation>().enabled = false;

                    Weapon weapon = enemyGo.GetComponentInChildren<Weapon>();
                    //weapon.SetCooldown(Random.Range(0.5f, agentsMaxCooldown));

                    BaseStats enemyStats = enemyGo.GetComponent<BaseStats>();
                    enemyStats.SetDefense(0);
                    //enemyStats.SetAttack(Random.Range(10, enemiesMaxAttack + 1));
                    enemyStats.SetAttack(1);
                    enemyStats.SetHealth(Random.Range(10, enemiesMaxHP + 1));
                    //enemyStats.SetSpeed(Random.Range(2, agentsMaxSpeed + 1));

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
        playerStats.SetAttack(Random.Range(10, playerMaxAttack + 1));
        //playerStats.SetSpeed(Random.Range(2, agentsMaxSpeed + 1));
        //playerStats.SetHealth(Random.Range(10, playerMaxHP + 1));

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
