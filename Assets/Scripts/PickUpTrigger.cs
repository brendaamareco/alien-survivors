using UnityEngine;

public class PickUpExp : MonoBehaviour
{
    [SerializeField] int experienceAmount = 10; // Amount of experience to give the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the colliding object is the player
            
            if (other.TryGetComponent<Player>(out var player))
            {
                // Increase the player's experience
                player.AddExperience(experienceAmount);
                Debug.Log("Player's experience: " + player.GetExperience());

                // Destroy the pickup object
                Destroy(gameObject);

            }
        }
    }
}
