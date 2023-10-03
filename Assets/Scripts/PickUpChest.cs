using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpChest : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ChestController chestController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the colliding object is the player

            if (other.TryGetComponent<Player>(out var player))
            {
                gameManager.SwitchLevelUp();
                chestController.Show();
            }
        }
    }
}
