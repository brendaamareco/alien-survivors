using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EffectStopTime : MonoBehaviour
{
    private GameObject prefabToSpawn;
    private float itemLifetime = 1f;
    public float stopTimeDuration = 5f;
    private bool isStopping = false; // Flag to track if time should be stopped.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEffect();
            transform.position += Vector3.up * 100f;
            Destroy(gameObject, stopTimeDuration+1f);
            isStopping = true;
            StartCoroutine(StopTime());
            StartCoroutine(ResumeTime());
            
            // Stop existing enemies
            StopEnemyMovement();
        }
    }

    IEnumerator ResumeTime()
    {
        // Wait for the stopTimeDuration.
        yield return new WaitForSeconds(stopTimeDuration);

        // Resume enemy movement
        isStopping = false;
        ResumeEnemyMovement();

        yield return new WaitForSeconds(1f);
    }

    IEnumerator StopTime()
    {
        while (isStopping)
        {
            StopEnemyMovement();
            yield return new WaitForSeconds(1f);
        }
    }

    // Function to stop enemy movement
    void StopEnemyMovement()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            BaseStats stats = enemy.GetComponent<BaseStats>();
            stats.m_Speed = -2;
        }
    }

    // Function to resume enemy movement
    void ResumeEnemyMovement()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            BaseStats stats = enemy.GetComponent<BaseStats>();
            stats.m_Speed = +2;
        }
    }

    public void SpawnEffect()
    {
        prefabToSpawn = Resources.Load<GameObject>("Effects/FreezeEffect");
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

