using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCheese : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                player.Heal(20);
                Destroy(gameObject);
            }
        }
    }

    public string GetName()
    { return typeof(PickUpBox).Name; }
}
