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
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Player"))
        {
            Agent agent = collision.gameObject.GetComponent<Agent>();

            if (agent)
            {
                Debug.Log(agent + " collisioned with wall");
                agent.AddReward(-0.001f);
                
            }
        }
    }
}
