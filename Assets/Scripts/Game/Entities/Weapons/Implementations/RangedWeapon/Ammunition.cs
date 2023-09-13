using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent (typeof(Rigidbody))]
public class Ammunition : MonoBehaviour
{
    [SerializeField] private float distance = 0;

    private int m_DamagePoints;
    private BoxCollider m_BoxCollider;
    private Vector3 m_OriginPosition;

    private void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = false;

        m_OriginPosition = transform.position;
    }

    private void Update()
    {

        float currentDistance =  Vector3.Distance(transform.position, m_OriginPosition);

        if (currentDistance > distance)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageableEntity damageable = collision.gameObject.GetComponentInChildren<DamageableEntity>();

        if (damageable != null)
            damageable.ReceiveDamage(m_DamagePoints);

        Destroy(gameObject);
    }

    public void SetDamagePoints(int damagePoints)
    { m_DamagePoints = damagePoints; }

    public void SetLayerMask(LayerMask layer)
    { gameObject.layer = layer; }

    public void SetDistance(float newDistance)
    { distance = newDistance; }
}
