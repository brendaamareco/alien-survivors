using UnityEngine;

public class PickUpChest : MonoBehaviour, IEntity
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                GameEventManager.GetInstance().Publish(GameEvent.CHEST_OPENED, new EventContext(this));
            }
        }
    }

    public string GetName()
    { return typeof(PickUpChest).Name; }
}
