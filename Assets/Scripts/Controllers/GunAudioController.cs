using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioController : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        GameEventManager.GetInstance().Suscribe(GameEvent.ATTACK, HandleAttackEvent);
    }

    void HandleAttackEvent(EventContext context)
    {
        StartCoroutine(PressForSeconds(0.5f));
    }

    IEnumerator PressForSeconds(float seconds)
    {
        m_AudioSource.Play();
        yield return new WaitForSeconds(seconds);
    }
}
