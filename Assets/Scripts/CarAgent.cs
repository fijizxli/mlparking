using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent
{
    //Rigidbody rigidbody;
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


        float moveSpeed = 2.0f;
        transform.position += new Vector3(moveX, 0, moveZ)*Time.deltaTime*moveSpeed;
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("new ep");
        transform.position = new Vector3(18,1,11);
    }

    private void OnCollisionEnter(Collision collision)
    {
    Debug.Log("new collision");

        if (collision.gameObject.CompareTag ("Car")) { 
            Debug.Log("car");
            SetReward(-1);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag ("PSpot")) { 
            SetReward(1);
            Debug.Log("Good job :)");
            StartCoroutine(WaitBeforeEndingEpisode(1f));
        }
        else if (collision.gameObject.CompareTag ("Helper")) { 
            AddReward(0.5f);
        }

    }

    IEnumerator WaitBeforeEndingEpisode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        EndEpisode();
    }

    void FixedUpdate() { 
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1);
            EndEpisode();
        }
        else if (this.transform.localPosition.x > 22 || this.transform.localPosition.x < -1 || this.transform.localPosition.z < -1 || this.transform.localPosition.z > 22)
        {
            SetReward(-1);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        if (Input.GetKey("w"))
            actions[1] = 1;

        if (Input.GetKey("s"))
            actions[1] = -1;

        if (Input.GetKey("d"))
            actions[0] = +1;

        if (Input.GetKey("a"))
            actions[0] = -1;
    }
}
