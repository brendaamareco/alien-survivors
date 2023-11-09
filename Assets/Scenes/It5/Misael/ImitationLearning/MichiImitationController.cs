using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class MichiImitationController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    [SerializeField] int MaxEnvironmentSteps = 25000;
    [SerializeField] BoxCollider spawnArea;
    private int m_ResetTimer;
    private Player m_player;
    private Enemy m_boss;
    private Agent m_agent;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameObject playerObject = GameObject.FindWithTag("Boss");
        if (playerObject != null)
        {
            m_boss = playerObject.GetComponent<Enemy>();
        }

        //m_boss_agent = GetComponentInChildren<Agent>();
        m_agent = m_player.transform.GetComponent<Agent>();
        if (m_agent)
        {
            Debug.Log("Agent encontrado");
        }
        else
        {
            Debug.Log("Agent no encontrado");
        }

        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_agent.AddReward(1f);
            m_agent.EpisodeInterrupted();
            ResetScene();
        }
    }

    private void HandleOnDead(EventContext context)
    {
        if (context.GetEntity().Equals(m_player))
        {
            m_agent.AddReward(-1f);
            m_agent.EndEpisode();
            ResetScene();
        }

        if (context.GetEntity().Equals(m_boss)) 
        {
            m_agent.AddReward(1f);
            m_agent.EndEpisode();
            ResetScene();
        }
    }

    private void HandleOnDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_player))
        {
            m_agent.AddReward(-1f);
        }
        if (context.GetEntity().Equals(m_boss))
        {
            m_agent.AddReward(1f);
        }
    }
    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_player.Heal(m_player.GetMaxHealthPoints());

        Bounds spawnerBounds = spawnArea.bounds;

        m_agent.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
        m_boss.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
    }
}
