using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent (typeof(Rigidbody))]
public class Ammunition : MonoBehaviour
{
    [SerializeField] float m_TimeAlive = 0f;
    [SerializeField] float m_DamagePoints = 0f;
    [SerializeField] LayerMask m_AttackerLayerMask;

    private BoxCollider m_BoxCollider;
    private Rigidbody m_Rigidbody;

    private void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = false;
        m_BoxCollider.excludeLayers = m_AttackerLayerMask;    

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire()
    { StartCoroutine(nameof(AmmunitionCoroutine)); }

    private IEnumerator AmmunitionCoroutine()
    {
        yield return new WaitForSeconds(m_TimeAlive);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageableEntity damageable = GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
        {
            damageable.ReceiveDamage(m_DamagePoints);
            Destroy(gameObject);
        }
    }

    public void SetTimeAlive(float time)
    { m_TimeAlive = time; }

    public void SetDamagePoints(float damagePoints)
    { m_DamagePoints = damagePoints; }

    public void SetAttackerLayerMask(LayerMask layer)
    { m_AttackerLayerMask = layer; }
}
