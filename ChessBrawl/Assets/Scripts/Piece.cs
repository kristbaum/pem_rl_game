using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public GameManager _gameManager = null;
    public enum PieceColor
    {
        Black,
        White
    }

    public MoveAgent agent; // Reference to the agent

    public float pushTimer = 0.0f;
    public const float pushDuration = 2.0f; // This duration can be adjusted as needed
    public bool wasKicked = false; // Flag to check if the piece was kicked
    public MoveAgent kickingAgent; // Reference to the agent that kicked this piece


    [Header("Piece Configuration")]
    [SerializeField] private PieceColor pieceColor = PieceColor.Black; // Default value
    [SerializeField] private float pieceValue = 10.0f; // Default value

    public PieceColor PieceColorValue
    {
        get { return pieceColor; }
    }

    public float PieceValue
    {
        get { return pieceValue; }
    }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetPiece()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;  // Reset linear velocity.
            rb.angularVelocity = Vector3.zero;  // Reset angular (rotational) velocity.
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this piece is in a "pushed" state
        if (pushTimer > 0.0f)
        {
            // Check for collision with another piece
            if (collision.gameObject.TryGetComponent<Piece>(out Piece hitPiece))
            {
                // If the hit piece is of the same color, propagate the "pushed" state
                if (hitPiece.PieceColorValue == this.PieceColorValue)
                {
                    hitPiece.SetPushedState();
                }
                // If of a different color, trigger the kick interaction if applicable
                else
                {
                    hitPiece.wasKicked = true; // Set the piece's kicked state
                    hitPiece.kickingAgent = this.agent; // Store the kicker's agent reference to the piece being kicked

                    // Assign the kicker's agent reference to the piece being kicked
                    hitPiece.agent = this.agent;

                    if (agent != null)
                    {
                        agent.AddReward(_gameManager.CRKickMotion);
                        _gameManager.currentScore++;
                        Debug.Log("Successful Kick Interaction! Reward: " + _gameManager.CRKickMotion);
                    }
                }
            }
        }
        // If this piece is in a "kicked" state
        else if (wasKicked)
        {
            // Check for collision with another piece
            if (collision.gameObject.TryGetComponent<Piece>(out Piece hitPiece))
            {
                // Only propagate the "kicked" state if the piece is of the same color
                if (hitPiece.PieceColorValue == this.PieceColorValue)
                {
                    hitPiece.wasKicked = true;
                    hitPiece.kickingAgent = this.kickingAgent;

                    // You can decide if you want to reassign the agent reference here
                    hitPiece.agent = this.agent;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check for wall collision.
        if (other.gameObject.CompareTag("Wall") && pushTimer > 0.0f)
        {
            if (agent != null)
            {
                agent.AddReward(_gameManager.CRTouchingOwnPiece);
                Debug.Log("Penalized agent with color: " + agent.AgentColorValue.ToString() + " for touching own piece " + this.gameObject.name + " with penalty: " + _gameManager.CRTouchingOwnPiece);
            }
        }


        if (other.gameObject.CompareTag("Wall") && wasKicked)
        {
            if (kickingAgent != null)
            {
                float reward = this.PieceValue;

                // Check the color of the piece that was kicked off
                if ((int)this.PieceColorValue == (int)kickingAgent.AgentColorValue)
                {
                    // Give a penalty if the agent kicked off a piece of its own color
                    kickingAgent.AddReward(-reward * _gameManager.CPKickingOffOwnPiece);  // assuming _gameManager.CRKickingOffSameColorPiece is a negative value representing the penalty
                    Debug.Log("Penalized agent with color: " + kickingAgent.AgentColorValue.ToString() + " for kicking off piece " + this.gameObject.name + " with same color: " + this.PieceColorValue.ToString() + " with penalty: " + -reward * _gameManager.CPKickingOffOwnPiece);
                }
                else
                {
                    // Give a reward for kicking off an opponent piece
                    kickingAgent.AddReward(reward * _gameManager.CRKickingOffOpponentPiece);
                    Debug.Log("Rewarded agent with color: " + kickingAgent.AgentColorValue.ToString() + " for kicking off piece " + this.gameObject.name + " with color: " + this.PieceColorValue.ToString() + " with reward: " + reward * _gameManager.CRKickingOffOpponentPiece);
                }

                // Reset the kicked state and kicking agent reference to prevent rewarding or penalizing multiple times
                wasKicked = false;
                kickingAgent = null;
            }
        }
    }

    // Define this function based on your color system. This example assumes only two colors (0 and 1).
    private int GetOppositeColorValue(int colorValue)
    {
        return colorValue == 0 ? 1 : 0;
    }

    public void SetPushedState()
    {
        pushTimer = pushDuration; // Set the timer to the full duration to make it "pushed"

        // Ensure there is a reference to the agent for the piece to report successful interactions
        if (agent == null)
        {
            agent = FindObjectOfType<MoveAgent>();
        }
    }

    void Update()
    {
        if (pushTimer > 0.0f)
        {
            pushTimer -= Time.deltaTime;
        }
        else if(agent != null)
        {
            agent = null;
        }
    }
}


