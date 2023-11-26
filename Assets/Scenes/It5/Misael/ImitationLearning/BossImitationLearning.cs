using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BossImitationLearning : Agent
{
    private Enemy m_Enemy;

    public override void Initialize()
    {
        m_Enemy = GetComponent<Enemy>();
    }
    /// <summary>
    /// Called every step of the engine. Here the agent takes an action.
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(m_Enemy.GetCurrentHealthPointsNormalized());
        sensor.AddObservation(transform.position);

        Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(player.GetCurrentHealthPointsNormalized());
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
        int forwardAxis = act[0];
        int rightAxis = act[1];

        float horizontalMove = 0f;
        float verticalMove = 0f;

        switch (forwardAxis)
        {
            case 1:
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

        m_Enemy.Move(new Vector3(horizontalMove, 0, verticalMove));
        m_Enemy.Attack(transform.position);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1; }

        if (Input.GetKey(KeyCode.DownArrow))
        { discreteActionsOut[0] = 2; }


        if (Input.GetKey(KeyCode.LeftArrow))
        { discreteActionsOut[1] = 1; }

        if (Input.GetKey(KeyCode.RightArrow))
        { discreteActionsOut[1] = 2; }
    }
}
