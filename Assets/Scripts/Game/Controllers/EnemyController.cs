using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyController : MonoBehaviour
{
    private Enemy m_Enemy;

    private void Start()
    { m_Enemy = GetComponent<Enemy>(); }

    private void FixedUpdate()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        m_Enemy.Move(playerTransform.position);
        m_Enemy.Attack(playerTransform.position);

        if (m_Enemy.GetCurrentHealthPoints() <= 0)
            Destroy(gameObject);
    }
}
