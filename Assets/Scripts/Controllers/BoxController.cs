using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BoxController : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public float itemLifetime = 3f;

    private PickUpBox m_PickUpBox;

    public void Start()
    {
        m_PickUpBox = GetComponent<PickUpBox>();
        GameEventManager.GetInstance().Suscribe(GameEvent.BOX_OPENED, HandleBoxOpened);
    }

    private void HandleBoxOpened(EventContext context)
    {
        try
        {
            PickUpBox box = (PickUpBox)context.GetEntity();

            //Si es la misma instancia que la caja, sino se abren todas las cajas
            if (box == m_PickUpBox)
            {
                SpawnRandomItem();
            }
        }
        catch { }
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

