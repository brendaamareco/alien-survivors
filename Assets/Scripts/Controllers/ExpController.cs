using System;
using UnityEngine;

public class ExpController : MonoBehaviour, IEntity
{  
    private int m_ExpNeeded = 20;

    private void Start()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.EXPERIENCE_CHANGED, HandleExperienceChanged);
    }

    private void HandleExperienceChanged(EventContext context)
    {
        try
        {
            Player player = (Player) context.GetEntity();
            CheckExp(player);
        }
        catch { }
    }

    private void CheckExp(Player player)
    {
        if (player.GetExperience() >= m_ExpNeeded)
        {
            GameEventManager.GetInstance().Publish(GameEvent.LEVEL_UP, new EventContext(player));
            m_ExpNeeded += m_ExpNeeded;
        }
    }

    public int GetExpNeeded()
    {  return m_ExpNeeded; }

    public string GetName()
    { return gameObject.name; }
}
