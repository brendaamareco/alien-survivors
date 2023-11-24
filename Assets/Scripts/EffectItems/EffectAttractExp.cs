using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpAttractor : MonoBehaviour
{
    public float attractionSpeed = 5f; // Adjust this value to control the attraction speed.
    private Transform playerTransform;

    private GameObject prefabToSpawn;
    private float itemLifetime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            SpawnEffect();
            transform.position += Vector3.up * 100f;
            Destroy(gameObject, itemLifetime);
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            AttractExpDrops();
        }
    }

    private void AttractExpDrops()
    {
        foreach (GameObject expDrop in GameObject.FindGameObjectsWithTag("Exp"))
        {
            Vector3 direction = playerTransform.position - expDrop.transform.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                float step = attractionSpeed * Time.deltaTime;
                expDrop.transform.position = Vector3.MoveTowards(expDrop.transform.position, playerTransform.position, step);
            }
        }
    }
    public void SpawnEffect()
    {
        prefabToSpawn = Resources.Load<GameObject>("Effects/AttractEffect");
        if (prefabToSpawn != null)
        {
            GameObject spawnedItem = Instantiate(prefabToSpawn, transform.position + Vector3.up, Quaternion.identity);
            Destroy(spawnedItem, itemLifetime);
        }
        else
        {
            Debug.LogError("No prefabs to spawn. Add prefabs to the 'prefabsToSpawn' array in the Inspector.");
        }
    }
}
