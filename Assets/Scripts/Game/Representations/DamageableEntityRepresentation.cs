using UnityEngine;

[RequireComponent(typeof(DamageableEntity))]
[RequireComponent (typeof(Animator))]
public class DamageableEntityRepresentation : MonoBehaviour
{
    [SerializeField] string animationNameReceiveDamage = "ReceiveDamage";

    private DamageableEntity m_Damageable;
    private Animator m_Animator;

    void Start()
    {
        m_Damageable = GetComponent<DamageableEntity>();
        m_Animator = GetComponent<Animator>();

        GameEventManager.GetInstance().Suscribe(GameEvent.DAMAGE, HandleDamage);
    }

    private void HandleDamage(EventContext context)
    {
        if (context.GetEntity().Equals(m_Damageable))
        {
            m_Animator.Play(animationNameReceiveDamage);
        }
    }
}
