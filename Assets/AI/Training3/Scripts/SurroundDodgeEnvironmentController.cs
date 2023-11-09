using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

public class SurroundDodgeEnvironmentController : MonoBehaviour
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

        //CheckEnemiesDistanceToPlayer();

        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            //Debug.Log("Enmemies:_" + totalEnemies);
            
            //Debug.Log("timeout. player health: " + m_Player.GetCurrentHealthPointsNormalized());

            m_GroupEnemy.AddGroupReward(-1f);
            m_GroupPlayer.AddGroupReward(-1f);

            m_GroupEnemy.EndGroupEpisode();
            m_GroupPlayer.EndGroupEpisode();

            floorMeshRenderer.material = tieMaterial;
            ResetScene();
        }
    }

    private void CheckEnemiesDistanceToPlayer()
    {
        foreach(Agent enemyAgent in enemyAgents)
        {
            float desiredDistance = enemyAgent.GetComponentInChildren<Weapon>().GetScope();
            float currentDistance = Vector3.Distance(enemyAgent.transform.position, m_Player.transform.position);

            if (currentDistance <= desiredDistance && currentDistance >= desiredDistance/2)
                enemyAgent.AddReward(1f / (float) MaxEnvironmentSteps);
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
            //agent.transform.position = new Vector3(
            //Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            //Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
            //);

            agent.transform.localPosition = spawnArea.transform.localPosition + new Vector3(Random.Range(1f, 3f), 0, 0);
        }

        //m_Player.transform.position = new Vector3(
        //    Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
        //    Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        //    );
        m_Player.transform.localPosition = spawnArea.transform.localPosition + new Vector3(0f, 0, Random.Range(1f,3f));
    }

    private void HandleOnAttack(EventContext context)
    {
        try
        {
            Weapon weapon = (Weapon)context.GetEntity();
            Agent agent = weapon.gameObject.GetComponentInParent<Agent>();


            if (agent && agent.CompareTag("Enemy") && m_GroupEnemy.GetRegisteredAgents().Contains(agent))
            {
                //Enemy enemy = agent.GetComponent<Enemy>();
                //int maxPossibleAttacks = m_Player.GetMaxHealthPoints() / enemy.GetAttackPoints();

                //if (maxPossibleAttacks == 0)
                //    maxPossibleAttacks = 1;

                //agent.AddReward( 2f / (float) maxPossibleAttacks );
                agent.AddReward(1f * m_GroupEnemy.GetRegisteredAgents().Count);

                //Debug.Log("Enemy attack success. maxPossibleAttacks: " + maxPossibleAttacks);
            }

            else if (agent && agent.CompareTag("Player") && m_GroupPlayer.GetRegisteredAgents().Contains(agent))
            {
                agent.AddReward( 1f / (float)m_GroupEnemy.GetRegisteredAgents().Count );
                //Debug.Log("Player attack success");
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


            if (agent && agent.CompareTag("Enemy") && m_GroupEnemy.GetRegisteredAgents().Contains(agent))
            {
                //Enemy enemy = agent.GetComponent<Enemy>();
                //int maxPossibleAttacks = enemy.GetMaxHealthPoints() / m_Player.GetAttackPoints();

                //if (maxPossibleAttacks == 0)
                //    maxPossibleAttacks = 1;

                //agent.AddReward(-0.1f / (float)maxPossibleAttacks);

                agent.AddReward(-0.1f);

                //Debug.Log("Enemy receive damage. maxPossibleAttacks: " + maxPossibleAttacks );
            }

            else if (agent && agent.CompareTag("Player") && m_GroupPlayer.GetRegisteredAgents().Contains(agent))
            {
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
                //Debug.Log("Player dead");

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
                    playerAgent.AddReward( 1f / (float)m_GroupEnemy.GetRegisteredAgents().Count);

                    //Debug.Log("Enemy dead");
                    //Debug.Log("#" + totalEnemies);

                    //m_GroupEnemy.AddGroupReward(-0.1f / (float)m_GroupEnemy.GetRegisteredAgents().Count);
                    //m_GroupPlayer.AddGroupReward(1f);

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