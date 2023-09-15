using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyController : MonoBehaviour
{
    private Enemy m_Enemy;
    public GameObject ExpModel;

    private void Start()
    { m_Enemy = GetComponent<Enemy>(); }

    private void FixedUpdate()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 targetPosition = playerTransform.position;

        m_Enemy.Move(targetPosition);
        m_Enemy.Rotate(targetPosition);
        m_Enemy.Attack(targetPosition);

        if (m_Enemy.GetCurrentHealthPoints() <= 0)
        {
            Destroy(gameObject);
            DropExp();
        }
    }

    private void DropExp()
    {
        Vector3 position = m_Enemy.transform.position;
        GameObject exp = Instantiate(ExpModel, position, Quaternion.identity);
    }
}
