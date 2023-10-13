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

    private void Start()
    { 
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_Group = new SimpleMultiAgentGroup();

        foreach ( GameObject enemyGameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Agent agent = enemyGameObject.transform.GetComponent<Agent>();

            if (agent)
                m_Group.RegisterAgent(agent);
        }

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleOnDamage);
        GameEventManager.GetInstance().Suscribe(GameEvent.DEAD, HandleOnDead);
        ResetScene();
    }

    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_Group.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        m_ResetTimer = 0;
        m_Player.Heal(m_Player.GetMaxHealthPoints());
        
        Bounds spawnerBounds = spawnArea.bounds;

        foreach (Agent agent in m_Group.GetRegisteredAgents())
        {         
            agent.transform.position = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x),0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
            );
        }
    }

    private void HandleOnDamage(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            m_Group.AddGroupReward(1f);

            Debug.Log("Player health:" + player.GetCurrentHealthPoints());
        }
        catch {}
    }

    private void HandleOnDead(EventContext context)
    {
        try
        {
            Player player = (Player)context.GetEntity();
            Debug.Log("Player dead");
            m_Group.AddGroupReward(1f);
            m_Group.EndGroupEpisode();
            ResetScene();
        }
        catch { }  
    }
}
