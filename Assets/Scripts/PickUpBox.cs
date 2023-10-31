using UnityEngine;

public class PickUpBox : MonoBehaviour, IEntity
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                GameEventManager.GetInstance().Publish(GameEvent.BOX_OPENED, new EventContext(this));
            }
        }
    }

    public string GetName()
    { return typeof(PickUpBox).Name; }
}
