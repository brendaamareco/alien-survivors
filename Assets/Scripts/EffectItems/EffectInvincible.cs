using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInvincible : MonoBehaviour
{
    private GameObject prefabToSpawn;
    private float itemLifetime = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEffect();
            GameObject playerObject = GameObject.FindWithTag("Player");
            Player player = playerObject.GetComponent<Player>();
            player.MakeInvincible();
            Destroy(gameObject);
        }
    }
    public void SpawnEffect()
    {
        prefabToSpawn = Resources.Load<GameObject>("Effects/ShieldEffect");
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
