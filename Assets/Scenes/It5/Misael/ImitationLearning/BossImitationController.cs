using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class BossImitationController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    [SerializeField] int MaxEnvironmentSteps = 25000;
    [SerializeField] BoxCollider spawnArea;
    private int m_ResetTimer;
    private Player m_Player;
    private Enemy m_boss;
    private Agent m_boss_agent;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameObject playerObject = GameObject.FindWithTag("Boss");
        if (playerObject != null)
        {
            m_boss = playerObject.GetComponent<Enemy>();
        }

        //m_boss_agent = GetComponentInChildren<Agent>();
        m_boss_agent = m_boss.transform.GetComponent<Agent>();
        if (m_boss_agent)
        {
            Debug.Log("Agent encontrado");
        }
        else {
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
            m_boss_agent.AddReward(1f);
            m_boss_agent.EpisodeInterrupted();
            ResetScene();
        }
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            m_boss_agent.AddReward(1f);
            m_boss_agent.EndEpisode();
            ResetScene();
        }
        catch { }
    }

    private void HandleOnDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_boss))
        {
            m_boss_agent.AddReward(-1f);
        }
    }
    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_Player.Heal(m_Player.GetMaxHealthPoints());

        Bounds spawnerBounds = spawnArea.bounds;

        m_boss_agent.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
        m_Player.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
    }
}
