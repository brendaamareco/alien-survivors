using UnityEngine;

public class PickUpChest : MonoBehaviour, IEntity
{
    [SerializeField] GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                gameManager.SwitchLevelUp();
                GameEventManager.GetInstance().Publish(GameEvent.CHEST_OPENED, new EventContext(this));
            }
        }
    }

    public string GetName()
    { return typeof(PickUpChest).Name; }
}
