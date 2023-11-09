using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    { AddReward(collision); }

    private void AddReward(Collision collision)
    {
        Agent agent = collision.gameObject.GetComponent<Agent>();

        if (agent)
            agent.AddReward(-0.001f);
    }
}
