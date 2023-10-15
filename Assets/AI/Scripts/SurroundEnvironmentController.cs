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
    private float m_DistanceMean;

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

        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleOnAttack);
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;

        float stepPlayerDistanceMean = 0;
        foreach(Agent agent in m_Group.GetRegisteredAgents())
        { 
            float playerDistance = Vector3.Distance(agent.transform.position, m_Player.transform.position);
            stepPlayerDistanceMean += playerDistance;
        }

        stepPlayerDistanceMean /= (float) m_Group.GetRegisteredAgents().Count;
        m_DistanceMean += stepPlayerDistanceMean;

        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_DistanceMean /= (float)MaxEnvironmentSteps;

            m_Group.AddGroupReward(-0.01f * m_DistanceMean);

            Debug.Log("Distance Mean: " + m_DistanceMean);
            
            m_Group.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        m_AgentsAttacking.Clear();
        m_ResetTimer = 0;
        m_DistanceMean = 0;
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

            enemyAgent.AddReward(1f);

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

    private void HandleOnDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            Debug.Log("Player dead");
            float attacksMean = 0f;

            foreach(Agent agent in m_AgentsAttacking.Keys)
            { attacksMean += m_AgentsAttacking[agent]; }

            attacksMean /= (float) m_Group.GetRegisteredAgents().Count;
            m_DistanceMean /= (float) MaxEnvironmentSteps;
            float enemyAttackedRatio = (float) m_AgentsAttacking.Keys.Count / (float) m_Group.GetRegisteredAgents().Count;

            m_Group.AddGroupReward( 10f * enemyAttackedRatio * attacksMean * (1f / (1f + m_DistanceMean)) );
            Debug.Log("Average attacks: " + attacksMean);
            Debug.Log("Agents attacked: " +  m_AgentsAttacking.Keys.Count);
            Debug.Log("Ratio: " + enemyAttackedRatio.ToString("F3"));
            Debug.Log("Distance mean along steps: " + m_DistanceMean);
            m_Group.EndGroupEpisode();
            ResetScene();
        }
        catch { }
    }
}