using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class BossImitationController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    [SerializeField] int MaxEnvironmentSteps = 5000;
    [SerializeField] BoxCollider spawnArea;
    [SerializeField] Material greenMaterial;
    [SerializeField] Material redMaterial;
    [SerializeField] GameObject wall1;
    [SerializeField] GameObject wall2;
    [SerializeField] GameObject wall3;
    [SerializeField] GameObject wall4;

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
        //m_boss.Attack(m_player.transform.position);

        if (enemies != null) 
        {
            enemies.Clear();
        }        
        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("Enemy");
        //if (enemiesObject.Length == 0)
        //{
        //    m_agent.EndEpisode();
        //    ResetScene();
        //}
        foreach (GameObject enemyObject in enemiesObject)
        {
            Enemy m_enemy = enemyObject.GetComponent<Enemy>();
            enemies.Add(m_enemy);
        }

        bool estaDetrasDeAlgunEnemigo = false;
        float angle = 0f;
        foreach (Enemy enemy in enemies) 
        {
            if (EtIsCovered(m_boss.transform.position, enemy.transform.position, m_player.transform.position))
            {
                estaDetrasDeAlgunEnemigo = true;
                float enemyAngle = GetEtAngle(m_boss.transform.position, enemy.transform.position, m_player.transform.position);
                if (enemyAngle > angle) 
                {
                    angle = enemyAngle;
                }
            }
        }

        if (estaDetrasDeAlgunEnemigo) 
        {
            if (angle > 155f && angle < 160f)
            {
                m_agent.AddReward(1f);
            }
            if (angle > 160f && angle < 165f)
            {
                m_agent.AddReward(1.2f);
            }
            if (angle > 165f && angle < 170f)
            {
                m_agent.AddReward(1.4f);
            }
            if (angle > 170f && angle < 175f)
            {
                m_agent.AddReward(1.6f);
            }
            if (angle > 175f && angle < 180f)
            {
                m_agent.AddReward(1.8f);
            }

            Renderer rend1 = wall1.GetComponent<Renderer>();
            rend1.material = greenMaterial;
            Renderer rend2 = wall2.GetComponent<Renderer>();
            rend2.material = greenMaterial;
            Renderer rend3 = wall3.GetComponent<Renderer>();
            rend3.material = greenMaterial;
            Renderer rend4 = wall4.GetComponent<Renderer>();
            rend4.material = greenMaterial;
        }
        else 
        {
            m_agent.AddReward(-0.001f);

            Renderer rend1 = wall1.GetComponent<Renderer>();
            rend1.material = redMaterial;
            Renderer rend2 = wall2.GetComponent<Renderer>();
            rend2.material = redMaterial;
            Renderer rend3 = wall3.GetComponent<Renderer>();
            rend3.material = redMaterial;
            Renderer rend4 = wall4.GetComponent<Renderer>();
            rend4.material = redMaterial;
        }

        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_agent.AddReward(0.25f);
            m_agent.EpisodeInterrupted();
            ResetScene();
        }

        m_agent.AddReward(1f/MaxEnvironmentSteps); 
    }

    private bool EtIsCovered(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
    {
        bool isCloseToAnAlien = EtIsCloseToAnAlien(etPosition, alienPosition);
        bool isBehindToAnAlien = EtIsBehindAnAlien(etPosition, alienPosition, catPosition);

        if (isCloseToAnAlien && isBehindToAnAlien)
        {
            return true;
        }
        return false;
    }

    private bool EtIsCloseToAnAlien(Vector3 etPosition, Vector3 alienPosition) 
    {
        float distanciaMaxima = 6.0f;
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

    private bool EtIsBehindAnAlien(Vector3 etPosition, Vector3 alienPosition, Vector3 catPosition)
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
