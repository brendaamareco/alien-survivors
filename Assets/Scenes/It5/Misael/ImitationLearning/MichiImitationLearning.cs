using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MichiImitationLearning : Agent
{
    private Player m_player;

    public override void Initialize()
    {
        m_player = GetComponent<Player>();
    }
    /// <summary>
    /// Called every step of the engine. Here the agent takes an action.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        Enemy enemy = GameObject.FindGameObjectWithTag("Boss").transform.GetComponent<Enemy>();
        sensor.AddObservation(transform.position);
        sensor.AddObservation(enemy.transform.position);
        sensor.AddObservation(enemy.GetCurrentHealthPointsNormalized());
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);
    }
    /// <summary>
    /// Moves the agent according to the selected action.
    /// </summary>
    public void MoveAgent(ActionSegment<int> act)
    {
        Debug.Log("Move agent michi");
        int forwardAxis = act[0];
        int rightAxis = act[1];

        float horizontalMove = 0f;
        float verticalMove = 0f;

        switch (forwardAxis)
        {
            case 1:
                Debug.Log("Flecha arriba 1f");
                verticalMove = 1f;
                break;
            case 2:
                verticalMove = -1f;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                horizontalMove = -1f;
                break;
            case 2:
                horizontalMove = 1f;
                break;
        }

        m_player.Move(new Vector3(horizontalMove, 0, verticalMove));
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.U))
        {
            Debug.Log("Flecha arriba");
            discreteActionsOut[0] = 1;
        }

        if (Input.GetKey(KeyCode.J))
        { discreteActionsOut[0] = 2; }


        if (Input.GetKey(KeyCode.H))
        { discreteActionsOut[1] = 1; }

        if (Input.GetKey(KeyCode.K))
        { discreteActionsOut[1] = 2; }
    }
}
