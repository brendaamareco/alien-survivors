using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SurroundDodgeAgent : Agent
{
    [SerializeField] Enemy m_Enemy;
    [SerializeField] Weapon m_Weapon;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(m_Enemy.GetCurrentHealthPointsNormalized());
        sensor.AddObservation(m_Enemy.GetSpeedPoints());
        sensor.AddObservation(m_Enemy.transform.localScale);

        sensor.AddObservation(m_Weapon.GetScope());
        sensor.AddObservation(m_Weapon.GetCooldown());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (m_Enemy.GetCurrentHealthPointsNormalized() > 0)
            MoveAgent(actions.DiscreteActions);
        else
            return;
    }

    private void MoveAgent(ActionSegment<int> act)
    {
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

        Vector3 dirToGo = new Vector3(horizontalMove, 0, verticalMove);
        m_Enemy.Move(dirToGo);
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
