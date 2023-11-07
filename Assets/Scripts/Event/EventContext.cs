public class EventContext 
{
    private IEntity publisher;
    private LevelUpSlot levelUpSlot;
    private GameManager gameManager;

    public EventContext(IEntity publisher)
    { this.publisher = publisher; }

    public EventContext(LevelUpSlot levelUpSlot)
    {
        this.levelUpSlot = levelUpSlot;
    }

    public EventContext(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public IEntity GetEntity() { return publisher; }
}
