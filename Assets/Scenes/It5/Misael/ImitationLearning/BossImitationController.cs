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
    private Player m_player;
    private Enemy m_boss;
    private Agent m_agent;
    private List<Enemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Enemy>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesObject != null)
        {
            int cantidad = enemiesObject.Length;
            Debug.Log($"La cantidad de enemigos encontrados es: {cantidad}");
        }
        else
        {
            Debug.Log("No se encontraron enemigos.");
        }
        foreach (GameObject enemyObject in enemiesObject) 
        {
            Enemy m_enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(m_enemy);
        }

        GameObject playerObject = GameObject.FindWithTag("Boss");
        if (playerObject != null)
        {
            m_boss = playerObject.GetComponent<Enemy>();
        }

        //m_boss_agent = GetComponentInChildren<Agent>();
        m_agent = m_boss.transform.GetComponent<Agent>();
        if (m_agent)
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
        if (enemies != null) 
        {
            enemies.Clear();
        }        
        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesObject.Length == 0)
        {
            m_agent.EndEpisode();
            ResetScene();
        }
        foreach (GameObject enemyObject in enemiesObject)
        {
            Enemy m_enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(m_enemy);
        }

        bool estaDetrasDeAlgunEnemigo = false;
        foreach (Enemy enemy in enemies) 
        {
            if (EstaDetrasDelEnemigo(m_boss.transform.position, enemy.transform.position))
            {
                Debug.Log("El jefe está detrás del enemigo.");
                estaDetrasDeAlgunEnemigo = true;
                m_agent.AddReward(1f);
            }
        }

        if (!estaDetrasDeAlgunEnemigo) 
        {
            m_agent.AddReward(-1f);
        }

        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_agent.AddReward(1f);
            m_agent.EpisodeInterrupted();
            ResetScene();
        }

        m_agent.AddReward(1f/MaxEnvironmentSteps); 
    }

    // Función para determinar si el jefe está detrás del enemigo.
    private bool EstaDetrasDelEnemigo(Vector3 posicionJefe, Vector3 posicionEnemigo)
    {
        // Compara las posiciones en los ejes X y Z.
        if (posicionJefe.z < posicionEnemigo.z && Mathf.Abs(posicionJefe.x - posicionEnemigo.x) < 1.0f)
        {
            return true;
        }

        return false;
    }

    private void HandleOnDead(EventContext context)
    {
        if (context.GetEntity().Equals(m_boss))
        {
            m_agent.AddReward(-1f);
            m_agent.EndEpisode();
            ResetScene();
        }
        if (context.GetEntity().Equals(m_player))
        {
            m_agent.EndEpisode();
            ResetScene();
        }
    }

    private void HandleOnDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_boss))
        {
            m_agent.AddReward(-0.1f);
        }
    }
    private void ResetScene()
    {
        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in enemiesObject)
        {
            Enemy m_enemy = enemyObject.GetComponent<Enemy>();
            m_enemy.Heal(m_enemy.GetMaxHealthPoints());
        }

        m_ResetTimer = 0;
        m_player.Heal(m_player.GetMaxHealthPoints());
        m_boss.Heal(m_boss.GetMaxHealthPoints());

        Bounds spawnerBounds = spawnArea.bounds;

        m_agent.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
        m_player.transform.localPosition = new Vector3(
            Random.Range(spawnerBounds.min.x, spawnerBounds.max.x), 0,
            Random.Range(spawnerBounds.min.z, spawnerBounds.max.z)
        );
    }
}
