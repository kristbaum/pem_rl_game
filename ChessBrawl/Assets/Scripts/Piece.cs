using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceColor
    {
        Black,
        White
    }

    public MoveAgent agent; // Reference to the agent

    public float pushTimer = 0.0f;
    public const float pushDuration = 2.0f; // This duration can be adjusted as needed

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
                    if (agent != null)
                    {
                        agent.AddReward(50f);
                        Debug.Log("Successful Kick Interaction!");
                    }
                }
            }
        }
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
