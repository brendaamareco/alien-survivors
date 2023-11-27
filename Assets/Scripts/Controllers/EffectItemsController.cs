using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItemsController : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] GameObject EffectAttackUp;
    [SerializeField] GameObject EffectSpeedUp;
    [SerializeField] GameObject EffectDefenseUp;
    [SerializeField] GameObject EffectHealthUp;
    [SerializeField] GameObject EffectLevelUp;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_ATKUP, HandleAtkUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_SPDUP, HandleSpeedUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_DEFUP, HandleDefenseUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_HLTUP, HandleHealthUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.LEVEL_UP, HandleLevelUp);
    }

    private void HandleAtkUp(EventContext context)
    {
        try
        {
            Player other = (Player)context.GetEntity();

            if (other == player)
                EffectAttackUp.SetActive(true);
        }
        catch { }
    }
    private void HandleSpeedUp(EventContext context)
    {
        try
        {
            Player other = (Player)context.GetEntity();

            if (other == player)
                EffectSpeedUp.SetActive(true);
        }
        catch { }
    }
    private void HandleDefenseUp(EventContext context)
    {
        try
        {
            Player other = (Player)context.GetEntity();

            if (other == player)
                EffectDefenseUp.SetActive(true);
        }
        catch { } 
    }
    private void HandleHealthUp(EventContext context)
    {
        try
        {
            Player other = (Player)context.GetEntity();

            if (other == player)
                EffectHealthUp.SetActive(true);
        }
        catch { }
    }
    private void HandleLevelUp(EventContext context)
    {
        try
        {
            Player other = (Player)context.GetEntity();

            if (other == player)
                StartCoroutine(setOnAndOff());
        }
        catch { }
    }
    private IEnumerator setOnAndOff()
    {
        EffectLevelUp.SetActive(true);
        yield return new WaitForSeconds(1f);
        EffectLevelUp.SetActive(false);
    }
}
