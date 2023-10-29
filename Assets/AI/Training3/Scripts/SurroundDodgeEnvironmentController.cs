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


    private SimpleMultiAgentGroup m_GroupEnemy;
    private SimpleMultiAgentGroup m_GroupPlayer;
    private int m_ResetTimer;
    private Dictionary<Agent, int> m_EnemiesAttacking;
    private int m_DeadEnemies = 0;
    float totalEnemies;

    private void Start()
    {
        m_EnemiesAttacking = new Dictionary<Agent, int>();
        m_GroupEnemy = new SimpleMultiAgentGroup();
        m_GroupPlayer = new SimpleMultiAgentGroup();

        m_GroupPlayer.RegisterAgent(m_Player.GetComponent<Agent>());
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
            
            //Debug.Log("Enmemies:_" + totalEnemies);
            float enemiesAliveRatio = ((totalEnemies - (float)m_DeadEnemies) / totalEnemies);
            Debug.Log("Enmemies Alive ratio:_" + enemiesAliveRatio);

            Debug.Log("timeout. player health: " + m_Player.GetCurrentHealthPointsNormalized());
            m_GroupEnemy.AddGroupReward(-1f * m_Player.GetCurrentHealthPointsNormalized());
            m_GroupPlayer.AddGroupReward(1f);

            Debug.Log("Ended");
            m_GroupEnemy.EndGroupEpisode();
            m_GroupPlayer.EndGroupEpisode();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        m_EnemiesAttacking.Clear();
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
            agent.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
            );
        }

        m_Player.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
            );
    }

    private void HandleOnAttack(EventContext context)
    {
        try
        {
            Weapon weapon = (Weapon)context.GetEntity();
            Agent agent = weapon.gameObject.GetComponentInParent<Agent>();

            agent.AddReward(0.1f);

            if (agent && agent.CompareTag("Enemy"))
            {
                Debug.Log("Enemy attack success");
                if (m_EnemiesAttacking.ContainsKey(agent))
                    m_EnemiesAttacking[agent] += 1;
                else
                    m_EnemiesAttacking[agent] = 1;
            }

            else if (agent && agent.CompareTag("Player"))
            {
                Debug.Log("Player attack success");

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

            if (agent)
                agent.AddReward(-0.1f);

        }
        catch { }
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            if (player)
            {
                Debug.Log("Player dead");                
                float attacksMean = 0f;

                foreach(Agent agent in m_EnemiesAttacking.Keys)
                { attacksMean += m_EnemiesAttacking[agent]; }

                attacksMean /= (float) m_GroupEnemy.GetRegisteredAgents().Count;
                float enemyAttackedRatio = (float) m_EnemiesAttacking.Keys.Count / (float) m_GroupEnemy.GetRegisteredAgents().Count;
            
                float enemiesAliveRatio = ((totalEnemies - (float) m_DeadEnemies) / totalEnemies);

                m_GroupEnemy.AddGroupReward( 10f * enemiesAliveRatio * attacksMean);

                m_GroupPlayer.AddGroupReward(-1f * enemiesAliveRatio);
                //Debug.Log("Average attacks: " + attacksMean);
                //Debug.Log("Agents attacked: " +  m_EnemiesAttacking.Keys.Count);
                //Debug.Log("Enemies attacking Ratio: " + enemyAttackedRatio.ToString("F3"));
                Debug.Log("Enemies alive Ratio: " + enemiesAliveRatio.ToString("F3"));
                m_GroupEnemy.EndGroupEpisode();
                m_GroupPlayer.EndGroupEpisode();
                ResetScene();
            }

        }
        catch { }

        try
        {
            Enemy enemy = (Enemy)context.GetEntity();
            if (enemy)
            {
                Debug.Log("Enemy dead");

                Agent enemyAgent = enemy.gameObject.GetComponent<Agent>();

                m_GroupEnemy.AddGroupReward(-1f);
                m_GroupPlayer.AddGroupReward(1f);

                m_DeadEnemies += 1;

                enemy.transform.position = new Vector3(0,-5f, 0);

                if (m_DeadEnemies == totalEnemies)
                {
                    m_GroupPlayer.AddGroupReward(10f * m_Player.GetCurrentHealthPointsNormalized());
                    m_GroupEnemy.AddGroupReward(-2f * m_Player.GetCurrentHealthPointsNormalized());

                    m_GroupEnemy.EndGroupEpisode();
                    m_GroupPlayer.EndGroupEpisode();
                    ResetScene();
                }
            }
        }
        catch { }
    }
}