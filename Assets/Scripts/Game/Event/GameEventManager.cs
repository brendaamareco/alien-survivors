using System;
using System.Collections.Generic;

public class GameEventManager
{
    private static GameEventManager Instance;
    private static Dictionary<GameEvent, List<Action<EventContext>>> m_EventHub;

    public static GameEventManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GameEventManager();
            m_EventHub = new Dictionary<GameEvent, List<Action<EventContext>>>();
        }

        return Instance;
    }

    public void Suscribe(GameEvent gameEvent, Action<EventContext> eventHandler)
    {
        if (m_EventHub.ContainsKey(gameEvent))
            m_EventHub[gameEvent].Add(eventHandler);
        else
            m_EventHub[gameEvent] = new List<Action<EventContext>> { eventHandler };
    }

    public void Publish(GameEvent gameEvent, EventContext context)
    {
        if (m_EventHub.ContainsKey(gameEvent))
        {
            List<Action<EventContext>> eventHandlers = m_EventHub[gameEvent];
            eventHandlers.ForEach( handler => handler.Invoke(context));
        }
    }

    public void Reset()
    { m_EventHub = new Dictionary<GameEvent, List<Action<EventContext>>>(); }
}
