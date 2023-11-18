using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerSurroundDodgeAgent : Agent
{
    [SerializeField] Player m_Player;

    public override void CollectObservations(VectorSensor sensor)
    {
        Weapon weapon = m_Player.GetComponentInChildren<Weapon>();

        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(m_Player.GetCurrentHealthPointsNormalized());
        sensor.AddObservation(m_Player.GetSpeedPoints());
        sensor.AddObservation(transform.localScale);

        //TODO: null error
        sensor.AddObservation(weapon.GetScope());
        sensor.AddObservation(weapon.GetCooldown());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (m_Player.GetCurrentHealthPointsNormalized() > 0)
            MoveAgent(actions.DiscreteActions);
        else
            return;
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        Vector3 dirToGo = Vector3.zero;
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

        m_Player.Move(new Vector3(horizontalMove, 0, verticalMove));
        m_Player.Attack(transform.position);
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
