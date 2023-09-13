using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent (typeof(Rigidbody))]
public class Ammunition : MonoBehaviour
{
    private int m_DamagePoints;
    private BoxCollider m_BoxCollider;

    private void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageableEntity damageable = collision.gameObject.GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
        {
            damageable.ReceiveDamage(m_DamagePoints);
            Destroy(gameObject);
        }
    }

    public void SetDamagePoints(int damagePoints)
    { m_DamagePoints = damagePoints; }

    public void SetLayerMask(LayerMask layer)
    { gameObject.layer = layer; }
}
