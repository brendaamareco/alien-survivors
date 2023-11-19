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
                Debug.Log($"El valor de game object name es: {gameObject.name}");
                if (gameObject.name == "Tuna" || gameObject.name == "Tuna(Clone)")
                {
                    GameEventManager.GetInstance().Publish(GameEvent.FINISH_LEVEL, new EventContext(player));
                }
                else 
                {
                    // Increase the player's experience
                    GameEventManager.GetInstance().Publish(GameEvent.EXPUP, new EventContext(player));//Chequear esto
                    player.AddExperience(experienceAmount);
                    Debug.Log("Player's experience: " + player.GetExperience());
                }
                Destroy(gameObject);
            }
        }
    }
}
