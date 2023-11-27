using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectKillEnemies : MonoBehaviour
{
    private GameObject prefabToSpawn;
    private float itemLifetime = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEffect();
            DestroyEnemies();
            Destroy(gameObject);
        }
    }

    private void DestroyEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Enemy e = enemy.GetComponent<Enemy>();
            e.ReceiveDamage(10000);
            //Destroy(enemy);
        }
    }

    public void SpawnEffect()
    {
        prefabToSpawn = Resources.Load<GameObject>("Effects/Explosion");
        if (prefabToSpawn != null)
        {
            GameObject spawnedItem = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            Destroy(spawnedItem, itemLifetime);
        }
        else
        {
            Debug.LogError("No prefabs to spawn. Add prefabs to the 'prefabsToSpawn' array in the Inspector.");
        }
    }
}
