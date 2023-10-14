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

    private void Update()
    {
        if (m_Enemy.GetCurrentHealthPoints() <= 0)
        {
            //DropExp();
            this.gameObject.SetActive(false);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);
        CheckRayCast();
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

    private void CheckRayCast()
    {
        RayPerceptionSensorComponent3D m_rayPerceptionSensorComponent3D = GetComponent<RayPerceptionSensorComponent3D>();

        var rayOutputs = RayPerceptionSensor.Perceive(m_rayPerceptionSensorComponent3D.GetRayPerceptionInput()).RayOutputs;
        int lengthOfRayOutputs = rayOutputs.Length;
        bool detectedPlayer = false;

        for (int i = 0; i < lengthOfRayOutputs; i++)
        {
            GameObject goHit = rayOutputs[i].HitGameObject;
            if (goHit != null)
            {
                var rayDirection = rayOutputs[i].EndPositionWorld - rayOutputs[i].StartPositionWorld;
                var scaledRayLength = rayDirection.magnitude;
                float rayHitDistance = rayOutputs[i].HitFraction * scaledRayLength;

                //if (goHit.CompareTag("Enemy") && rayHitDistance < 0.1f)
                //    AddReward(-0.01f);

                if (goHit.CompareTag("Player"))
                {
                    detectedPlayer = true;

                    SetReward(0.1f / (1 + rayHitDistance));
                    break;
                    //if (rayHitDistance < 5f)
                       //SetReward(0.1f * (1 / (1 + rayHitDistance)));
                }
            }
        }

        //if (!detectedPlayer)
        //    AddReward(-0.01f);
    }
}
