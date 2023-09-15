public class EventContext 
{
    private IEntity publisher;

    public EventContext(IEntity publisher)
    { this.publisher = publisher; }

    public IEntity GetEntity() { return publisher; }
}
