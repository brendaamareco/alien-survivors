using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlinderComponent : WeaponComponent
{
    [SerializeField] string postProcessLayerTag = "MainCamera";
    [SerializeField] LayerMask effectLayer;

    private PostProcessLayer m_PostProcessLayer;
    private LayerMask previousPostProcessLayer;
    private bool m_IsCoroutineRunning = false;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag(postProcessLayerTag);
        m_PostProcessLayer = go.GetComponent<PostProcessLayer>();
    }

    public override void HandleOnHit(DamageableEntity damageable) {}

    public override void HandleOnHit(Player player)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < GetProbability() && !m_IsCoroutineRunning)
            StartCoroutine(nameof(BlindCoroutine));
        else
            GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }

    private IEnumerator BlindCoroutine()
    {
        m_IsCoroutineRunning = true;

        if (m_PostProcessLayer != null)
        {
            previousPostProcessLayer = m_PostProcessLayer.volumeLayer;
            m_PostProcessLayer.volumeLayer = effectLayer;
            yield return new WaitForSeconds(GetDuration());
            m_PostProcessLayer.volumeLayer = previousPostProcessLayer;
        }

        else
            yield return new WaitForSeconds(GetDuration());

        m_IsCoroutineRunning = false;
        GameEventManager.GetInstance().Publish(GameEvent.WEAPON_COMPONENT_END, new EventContext(this));
    }
}
