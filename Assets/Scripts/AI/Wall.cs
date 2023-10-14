using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    { AddReward(collision); }

    private void OnCollisionStay(Collision collision)
    { AddReward(collision); }

    private void AddReward(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Agent enemyAgent = collision.gameObject.GetComponent<Agent>();

            if (enemyAgent)
                enemyAgent.AddReward(-0.01f);
        }
    }
}
