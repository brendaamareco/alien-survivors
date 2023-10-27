using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    private Enemy m_Enemy;
    public GameObject ExpModel;

    private void Awake()
    { m_Enemy = GetComponent<Enemy>(); }

    private void FixedUpdate()
    {
        if (m_Enemy.GetCurrentHealthPoints() <= 0)
        {
            DropExp();
            Destroy(gameObject);
        }
    }

    private void DropExp()
    { Instantiate(ExpModel, transform.position, Quaternion.identity); }
}
