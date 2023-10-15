using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SurroundAgent : Agent
{
    private Enemy m_Enemy;

    public override void Initialize()
    {
        m_Enemy = GetComponent<Enemy>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player>();
        sensor.AddObservation(transform.position); 
        sensor.AddObservation(player.transform.position);
        sensor.AddObservation(player.GetCurrentHealthPointsNormalized());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        Vector3 dirToGo = Vector3.zero;
        int forwardAxis = act[0];
        int rightAxis = act[1];

        float horizontalMove = 0f;
        float verticalMove = 0f;

        switch(forwardAxis)
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

        m_Enemy.Move(new Vector3(horizontalMove, 0 , verticalMove));
        m_Enemy.Attack(transform.position);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        { discreteActionsOut[0] = 1; }

        if (Input.GetKey(KeyCode.S))
        { discreteActionsOut[0] = 2; }


        if (Input.GetKey(KeyCode.A))
        { discreteActionsOut[1] = 1; }
        
        if (Input.GetKey(KeyCode.D))
        { discreteActionsOut[1] = 2; }
    }
}
