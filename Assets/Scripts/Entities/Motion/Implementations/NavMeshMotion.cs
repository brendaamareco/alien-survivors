using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class NavMeshMotion : Motion
{
    private NavMeshAgent m_Agent;

    private void Start()
    { 
        m_Agent = transform.GetComponentInChildren<NavMeshAgent>();
    }

    public override void Move(Vector3 vector, float speed)
    {
        m_Agent.speed = speed;
        m_Agent.SetDestination(vector);
    }
}
