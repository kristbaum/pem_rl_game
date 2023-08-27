using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveAgent : Agent {

    public enum AgentColor
    {
        Black,
        White
    }


    [Header("Agent Configuration")]
    [SerializeField] private AgentColor agentColor = AgentColor.Black; // Default value

    public List<Transform> targetTransforms = new List<Transform>();

    [Header("Initial Position Ranges")]
    [SerializeField] private float startXMin = 3f;
    [SerializeField] private float startXMax = 11.5f;
    [SerializeField] private float startYMin = 0f;
    [SerializeField] private float startYMax = 0f;
    [SerializeField] private float startZMin = -6.7f;
    [SerializeField] private float startZMax = -6.7f;

    public override void OnEpisodeBegin()
    {
        ResetAllPieces();

        transform.localPosition = new Vector3(
            Random.Range(startXMin, startXMax),
            Random.Range(startYMin, startYMax),
            Random.Range(startZMin, startZMax)
        );
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

    // Reward calculation for the agent.
    private void OnTriggerEnter(Collider other)
    {
        // Check for wall collision.
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-100f);
            EndEpisode();
        }

        // Check for piece collision.
        if(other.TryGetComponent<Piece>(out Piece piece))
        {
            // If agent collides with a piece of its own color.
            if((int)piece.PieceColorValue == (int)this.agentColor)
            {
                Debug.Log("Same Color Interaction Detected!");
                piece.agent = this; // Setting the reference
                piece.pushTimer = Piece.pushDuration; // Reference to the constant pushDuration from the Piece script
                AddReward(1f);
            }
            // If agent collides directly with a piece of the opposite color.
            else
            {
                // Penalize for touching the opposite piece directly.
                AddReward(-10f);
            }
        }
    }

    private void ResetAllPieces() 
    {
        foreach (Transform targetTransform in targetTransforms) 
        {
            Piece piece = targetTransform.GetComponent<Piece>();
            piece.ResetPiece();
        }
    }

    void Start()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach(GameObject piece in pieces)
        {
            targetTransforms.Add(piece.transform);
        }
    }

}
