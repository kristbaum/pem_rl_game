using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveAgent : Agent {

    //[SerializeField] private Transform targetTransform;
    public List<Transform> targetTransforms = new List<Transform>();

    public override void OnEpisodeBegin(){
        transform.localPosition = new Vector3(
            Random.Range(3f,11.5f),
            0f,
            -6.7f);
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.position);

        foreach (Transform targetTransform in targetTransforms)
        {
            sensor.AddObservation(targetTransform.position);
        }
           
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 1f;
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other){
        if(other.TryGetComponent<Wall>(out Wall wall)){
            AddReward(-100f);
            EndEpisode();
        }
        if(other.TryGetComponent<Piece>(out Piece piece)){
            AddReward(1f);
            EndEpisode();
        }
    }

}
