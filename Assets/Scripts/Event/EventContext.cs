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

    public EventContext(ChestController chestController)
    {
        ChestController = chestController;
    }

    public ChestController ChestController { get; }

    public IEntity GetEntity() { return publisher; }
}
