using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
// ReSharper disable InconsistentNaming

public class AgentPusher : Agent
{
    readonly float m_LateralSpeed = 0.15f;
    readonly float m_ForwardSpeed = 0.5f;


    [HideInInspector]
    public Rigidbody agentRb;
    
    public GameEnvController envController;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;


        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        dirToGo = rightAxis switch
        {
            1 => transform.right * m_LateralSpeed,
            2 => transform.right * -m_LateralSpeed,
            _ => forwardAxis switch
            {
                1 => transform.forward * m_ForwardSpeed,
                2 => transform.forward * -m_ForwardSpeed,
                _ => dirToGo
            }
        };

        rotateDir = rotateAxis switch
        {
            1 => transform.up * -1f,
            2 => transform.up * 1f,
            _ => rotateDir
        };

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // return;
        var value = envController.door.GetActiveValue() * 0.8f;
        // AddReward((-1f / MaxStep) * (1 - value));
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        envController.ResetScene();
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // sensor.AddObservation(transform.localPosition);
        // sensor.AddObservation(transform.localRotation.eulerAngles);
    }
}
