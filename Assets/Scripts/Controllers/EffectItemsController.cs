using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItemsController : MonoBehaviour
{
    [SerializeField] GameObject EffectAttackUp;
    [SerializeField] GameObject EffectSpeedUp;
    [SerializeField] GameObject EffectDefenseUp;
    [SerializeField] GameObject EffectHealthUp;
    [SerializeField] GameObject EffectLevelUp;

    void Start()
    {
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_ATKUP, HandleAtkUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_SPDUP, HandleSpeedUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_DEFUP, HandleDefenseUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.EFFECT_HLTUP, HandleHealthUp);
        GameEventManager.GetInstance().Suscribe(GameEvent.LEVEL_UP, HandleLevelUp);
    }

    private void HandleAtkUp(EventContext context)
    {
        EffectAttackUp.SetActive(true);
    }
    private void HandleSpeedUp(EventContext context)
    {
        EffectSpeedUp.SetActive(true);
    }
    private void HandleDefenseUp(EventContext context)
    {
        EffectDefenseUp.SetActive(true);
    }
    private void HandleHealthUp(EventContext context)
    {
        EffectHealthUp.SetActive(true);
    }
    private void HandleLevelUp(EventContext context)
    {
        StartCoroutine(setOnAndOff());

    }
    private IEnumerator setOnAndOff()
    {
        EffectLevelUp.SetActive(true);
        yield return new WaitForSeconds(1f);
        EffectLevelUp.SetActive(false);
    }
}
