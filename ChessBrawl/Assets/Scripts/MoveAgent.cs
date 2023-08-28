using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveAgent : Agent {

    public static List<MoveAgent> AllAgents;
    public GameManager _gameManager = null;
    public AudioSource audioPlayer;

    [Header("Environment Configuration")]
    [SerializeField] private GameObject board;

    public enum AgentColor
    {
        Black,
        White
    }

    [Header("Agent Configuration")]
    [SerializeField] private AgentColor agentColor = AgentColor.Black; // Default value

    public AgentColor AgentColorValue
    {
        get { return agentColor; }
    }

    public List<Transform> targetTransforms = new List<Transform>();

    [Header("Initial Position Ranges")]
    [SerializeField] private float startXMin = 3f;
    [SerializeField] private float startXMax = 11.5f;
    [SerializeField] private float startYMin = 0f;
    [SerializeField] private float startYMax = 0f;
    [SerializeField] private float startZMin = -6.7f;
    [SerializeField] private float startZMax = -6.7f;

    private void Awake()
    {
        if(AllAgents == null)
        {
            AllAgents = new List<MoveAgent>();
        }

        AllAgents.Add(this);
    }

    public override void OnEpisodeBegin()
    {
        // This agent will reset every agent, including itself
        foreach (MoveAgent agent in AllAgents)
        {
            agent.ResetAgent();
        }
    }

    private Rigidbody rb;
    private void ResetAgent()
    {
        ResetAllPieces();

        transform.localPosition = new Vector3(
            Random.Range(startXMin, startXMax),
            Random.Range(startYMin, startYMax),
            Random.Range(startZMin, startZMax)
        );

        //Reset agents rotation
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;  // Reset linear velocity.
            rb.angularVelocity = Vector3.zero;  // Reset angular (rotational) velocity.
        }
    }


    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.position);

        // Observing the board's position
        sensor.AddObservation(board.transform.position);

        foreach (Transform targetTransform in targetTransforms)
        {
            sensor.AddObservation(targetTransform.position);
        }
    }

    private float kickForce;
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = actions.ContinuousActions[2];
        float kickForce = actions.ContinuousActions[3];


        float moveSpeedMin = 0.5f;
        float moveSpeedMax = 5f;
        moveSpeed = moveSpeedMin + ((moveSpeed + 1) * (moveSpeedMax - moveSpeedMin) / 2);
        float kickForceMin = 2f;
        float kickForceMax = 10f;
        kickForce = kickForceMin + ((kickForce + 1) * (kickForceMax - kickForceMin) / 2);

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        // Lock X and Z rotation
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetAxisRaw("VerticalWAndS");
        continuousActions[3] = Input.GetAxisRaw("HorizontalAndD");
    }

    // Reward calculation for the agent.
    private void OnTriggerEnter(Collider other)
    {
        audioPlayer.Play();
        // Check for wall collision.
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(_gameManager.CPWall);
            EndEpisode();
        }

        // Check agent collision.
        if(other.TryGetComponent<MoveAgent>(out MoveAgent agent))
        {
            // Penalize for touching the opposite agent directly.
            AddReward(_gameManager.CPTouchingAgent);
            EndEpisode();
        }

        // Check for piece collision.
        if(other.TryGetComponent<Piece>(out Piece piece))
        {
             // Calculate kick direction.
            Vector3 kickDirection = (piece.transform.position - transform.position).normalized;

            // Apply force to the piece's Rigidbody.
            Rigidbody pieceRb = piece.GetComponent<Rigidbody>();
            if (pieceRb)
            {
                pieceRb.AddForce(kickDirection * kickForce, ForceMode.Impulse);
            }

            // If agent collides with a piece of its own color.
            if((int)piece.PieceColorValue == (int)this.agentColor)
            {
                Debug.Log("Same Color Interaction Detected!");
                piece.agent = this; // Setting the reference
                piece.pushTimer = Piece.pushDuration; // Reference to the constant pushDuration from the Piece script
                AddReward(_gameManager.CRTouchingOwnPiece);
            }
            // If agent collides directly with a piece of the opposite color.
            else
            {
                // Penalize for touching the opposite piece directly.
                AddReward(_gameManager.CPTouchingOpponentPiece);
                Debug.Log("Opposite Color Interaction Detected!");
                //EndEpisode();
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
