using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    Rigidbody rigidbody;
    public Transform targetTransform;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];


        float moveSpeed = 1f;
        transform.position += new Vector3(moveX, 0, moveZ)*Time.deltaTime*moveSpeed;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(4,1,1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Wall"){
            AddReward(-1);
            EndEpisode();
        }
        else if (collision.tag == "Car") { 
            SetReward(-1);
            EndEpisode();
        }
        else if (collision.tag == "PSpot") { 
            SetReward(1);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        if (Input.GetKey("w"))
            actions[0] = 1;

        if (Input.GetKey("s"))
            actions[0] = -1;

        if (Input.GetKey("d"))
            actions[1] = +1f;

        if (Input.GetKey("a"))
            actions[1] = -1f;
    }
}
