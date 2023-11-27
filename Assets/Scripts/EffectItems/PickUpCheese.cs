using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCheese : MonoBehaviour
{
    private GameObject prefabToSpawn;
    private float itemLifetime = 0.8f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                player.Heal(50);
                Destroy(gameObject);
                SpawnEffect();
            }
        }
    }
    public void SpawnEffect()
    {
        prefabToSpawn = Resources.Load<GameObject>("Effects/HealEffect");
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

    public string GetName()
    { return typeof(PickUpBox).Name; }
}
