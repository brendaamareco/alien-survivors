using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoxController : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public float itemLifetime = 3f;

    public void Start()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.BOX_OPENED, HandleBoxOpened);
    }

    private void HandleBoxOpened(EventContext context)
    {

        SpawnRandomItem();
    }
    void SpawnRandomItem()
    {
        prefabsToSpawn = Resources.LoadAll<GameObject>("EffectItems");
        if (prefabsToSpawn.Length > 0)
        {
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            GameObject randomPrefab = prefabsToSpawn[randomIndex];

            // Spawn the random prefab above the box.
            GameObject spawnedItem = Instantiate(randomPrefab, transform.position + Vector3.up, Quaternion.identity);

            // Destroy the spawned item after 'itemLifetime' seconds.
            //Destroy(spawnedItem, itemLifetime);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("No prefabs to spawn. Add prefabs to the 'prefabsToSpawn' array in the Inspector.");
        }
    }
}

