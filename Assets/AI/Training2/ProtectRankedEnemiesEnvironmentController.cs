using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

public class ProtectRankedEnemiesEnvironmentController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    [SerializeField] int MaxEnvironmentSteps = 25000;
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] Agent[] enemyAgents;
    [SerializeField] Player m_Player;
    [SerializeField] Transform loserPlace;
    [SerializeField] private Material enemyWinMaterial;
    [SerializeField] private Material playerWinMaterial;
    [SerializeField] private Material tieMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    private SimpleMultiAgentGroup m_GroupEnemy;
    private SimpleMultiAgentGroup m_GroupPlayer;
    private Agent playerAgent;
    private int m_ResetTimer;
    private int m_DeadEnemies = 0;
    float totalEnemies;

    private void Start()
    {
        m_GroupEnemy = new SimpleMultiAgentGroup();
        m_GroupPlayer = new SimpleMultiAgentGroup();

        playerAgent = m_Player.GetComponent<Agent>();
        m_GroupPlayer.RegisterAgent(playerAgent);
        enemyAgents.ToList().ForEach(agent => m_GroupEnemy.RegisterAgent(agent));

        totalEnemies = (float)m_GroupEnemy.GetRegisteredAgents().Count;

        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleOnAttack);
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);

        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;

        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_GroupPlayer.AddGroupReward(-1f);
            CheckEnemiesAlive();

            m_GroupEnemy.EndGroupEpisode();
            m_GroupPlayer.EndGroupEpisode();

            floorMeshRenderer.material = tieMaterial;
            ResetScene();
        }
    }

    private void CheckEnemiesAlive()
    {
        foreach (Agent agent in m_GroupEnemy.GetRegisteredAgents())
        {
            Enemy enemy = agent.gameObject.GetComponent<Enemy>();

            if (enemy.GetCurrentHealthPointsNormalized() > 0)
            {
                if (enemy.gameObject.layer == LayerMask.NameToLayer("EnemyR2"))
                {
                    Debug.Log("enemy range 2 alive");
                    m_GroupEnemy.AddGroupReward(2f);
                }

                else if (enemy.gameObject.layer == LayerMask.NameToLayer("EnemyR3"))
                {
                    Debug.Log("enemy range 3 alive");
                    m_GroupEnemy.AddGroupReward(3f);
                }
            }
        }
    }

    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_DeadEnemies = 0;

        m_Player.Heal(m_Player.GetMaxHealthPoints());

        foreach (Agent agent in enemyAgents)
        {
           agent.gameObject.SetActive(true);
           Enemy enemy = agent.GetComponent<Enemy>();
           enemy.Heal(enemy.GetMaxHealthPoints());
        }

        Bounds spawnerBounds = spawnArea.bounds;

        foreach (Agent agent in m_GroupEnemy.GetRegisteredAgents())
        {
            agent.transform.localPosition = spawnArea.transform.localPosition + new Vector3(Random.Range(3f, 6f), 0, 0);
        }

        m_Player.transform.localPosition = spawnArea.transform.localPosition + new Vector3(0f, 0, Random.Range(3f,6f));
    }

    private void HandleOnAttack(EventContext context)
    {
        try
        {
            Weapon weapon = (Weapon)context.GetEntity();
            Agent agent = weapon.gameObject.GetComponentInParent<Agent>();


            if (agent && agent.CompareTag("Enemy") && m_GroupEnemy.GetRegisteredAgents().Contains(agent))
            {
                Debug.Log("enemy attacked");
                agent.AddReward(1f * m_GroupEnemy.GetRegisteredAgents().Count);
            }

            else if (agent && agent.CompareTag("Player") && m_GroupPlayer.GetRegisteredAgents().Contains(agent))
            {
                Debug.Log("player attacked");
                agent.AddReward( 1f / (float)m_GroupEnemy.GetRegisteredAgents().Count );
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

            if (agent && m_GroupEnemy.GetRegisteredAgents().Contains(agent))
            {
                if (agent.CompareTag("Enemy"))
                {
                    agent.AddReward(-0.1f);

                    if (agent.gameObject.layer == LayerMask.NameToLayer("EnemyR2"))
                    {
                        Debug.Log("enemy range 2 damaged");
                        m_GroupEnemy.AddGroupReward(-0.25f);
                    }

                    if (agent.gameObject.layer == LayerMask.NameToLayer("EnemyR3"))
                    {
                        Debug.Log("enemy range 3 damaged");
                        m_GroupEnemy.AddGroupReward(-0.5f);
                    }
                }      
            }

            else if (agent && agent.CompareTag("Player") && m_GroupPlayer.GetRegisteredAgents().Contains(agent))
            {
                Debug.Log("player damaged");
                agent.AddReward(-0.1f);
            }

        }
        catch { }
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();

            if (player && player == m_Player)
            {
                playerAgent.AddReward(-1f);

                m_GroupEnemy.AddGroupReward(1f);
                m_GroupPlayer.AddGroupReward(-1f);

                m_GroupEnemy.EndGroupEpisode();
                m_GroupPlayer.EndGroupEpisode();

                floorMeshRenderer.material = enemyWinMaterial;
                ResetScene();
            }

        }
        catch { }

        try
        {
            Enemy enemy = (Enemy)context.GetEntity();

            if (enemy)
            {
                Agent enemyAgent = enemy.gameObject.GetComponent<Agent>();
        
                if (m_GroupEnemy.GetRegisteredAgents().Contains(enemyAgent))
                {
                    enemyAgent.AddReward(-1f / (float)m_GroupEnemy.GetRegisteredAgents().Count);
                    playerAgent.AddReward(1f / (float)m_GroupEnemy.GetRegisteredAgents().Count);

                    m_GroupPlayer.AddGroupReward(1f / (float)m_GroupEnemy.GetRegisteredAgents().Count);

                    if (enemyAgent.gameObject.layer == LayerMask.NameToLayer("EnemyR2"))
                    {
                        m_GroupEnemy.AddGroupReward(-2f);
                    }

                    else if (enemyAgent.gameObject.layer == LayerMask.NameToLayer("EnemyR3"))
                    {
                        m_GroupEnemy.AddGroupReward(-3f);
                    }

                    m_DeadEnemies += 1;

                    enemy.transform.position = loserPlace.position;

                    if (m_DeadEnemies == totalEnemies)
                    {
                        m_GroupPlayer.AddGroupReward(1f);
                        m_GroupEnemy.AddGroupReward(-1f);

                        m_GroupEnemy.EndGroupEpisode();
                        m_GroupPlayer.EndGroupEpisode();

                        floorMeshRenderer.material = playerWinMaterial;
                        ResetScene();
                    }
                }              
            }
        }
        catch { }
    }
}