using UnityEngine;

public class PickUpExp : MonoBehaviour
{
    [SerializeField] int experienceAmount = 10; // Amount of experience to give the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CatOrange")
        {
            // Check if the colliding object is the player
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                // Increase the player's experience
                player.m_Experience += experienceAmount;
                Debug.Log("Player's experience: " + player.m_Experience);

                // Destroy the pickup object
                Destroy(gameObject);

            }
        }
    }
}
