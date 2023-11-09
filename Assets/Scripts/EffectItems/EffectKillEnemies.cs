using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKillEnemies : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyEnemies();
        }
    }

    private void DestroyEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }
}
