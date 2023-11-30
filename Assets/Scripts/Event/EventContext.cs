public class EventContext 
{
    private IEntity publisher;
    private LevelUpSlot levelUpSlot;
    private GameManager gameManager;
    private float damageAmount;

    public EventContext(IEntity publisher)
    { this.publisher = publisher; }

    public EventContext(IEntity publisher, float damageAmount)
    { 
        this.publisher = publisher;
        this.damageAmount = damageAmount;
    }
    
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
    public float GetDamageAmount()
    {
        return damageAmount;
    }
}
