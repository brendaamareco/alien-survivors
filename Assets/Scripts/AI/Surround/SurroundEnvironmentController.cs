using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;


public class SurroundEnvironmentController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    [SerializeField] int MaxEnvironmentSteps = 25000;
    [SerializeField] BoxCollider spawnArea;

    private SimpleMultiAgentGroup m_Group;
    private int m_ResetTimer;
    private Player m_Player;
    private Dictionary<Agent, int> m_AgentsAttacking;

    private void Start()
    {
        m_AgentsAttacking = new Dictionary<Agent, int>();
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_Group = new SimpleMultiAgentGroup();

        foreach (GameObject enemyGameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Agent agent = enemyGameObject.transform.GetComponent<Agent>();

            if (agent)
                m_Group.RegisterAgent(agent);
        }

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleOnAttack);
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            float enemiesAttackedRatio = m_AgentsAttacking.Keys.Count / m_Group.GetRegisteredAgents().Count;
            m_Group.AddGroupReward(-500f * (1 - enemiesAttackedRatio));

            Debug.Log("enemies attacked: " + m_AgentsAttacking.Keys.Count);
            Debug.Log("total enemies: " + m_Group.GetRegisteredAgents().Count);
            Debug.Log("ratio: " +  enemiesAttackedRatio);

            m_Group.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        m_AgentsAttacking.Clear();
        m_ResetTimer = 0;
        m_Player.Heal(m_Player.GetMaxHealthPoints());

        Bounds spawnerBounds = spawnArea.bounds;

        foreach (Agent agent in m_Group.GetRegisteredAgents())
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
            Weapon weapon = (Weapon) context.GetEntity();
            Agent enemyAgent = weapon.gameObject.GetComponentInParent<Agent>();

            if (enemyAgent)
            {
                if (m_AgentsAttacking.ContainsKey(enemyAgent))
                    m_AgentsAttacking[enemyAgent] += 1;
                else
                    m_AgentsAttacking[enemyAgent] = 1;
            }
        }
        catch { }
    }

    private void HandleOnDamage(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            m_Group.AddGroupReward(1f);

            Debug.Log("Player health:" + player.GetCurrentHealthPoints());
        }
        catch { }
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            Debug.Log("Player dead");
            float averageAttacks = 0f;

            foreach(Agent agent in m_AgentsAttacking.Keys)
            { averageAttacks += m_AgentsAttacking[agent]; }

            averageAttacks /= m_AgentsAttacking.Keys.Count;

            m_Group.AddGroupReward(500 * m_AgentsAttacking.Keys.Count * averageAttacks);
            Debug.Log("Average attacks: " + averageAttacks);
            Debug.Log("Agents attacked: " +  m_AgentsAttacking.Keys.Count);
            m_Group.EndGroupEpisode();
            ResetScene();
        }
        catch { }
    }
}