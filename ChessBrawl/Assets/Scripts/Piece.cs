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
    public const float pushDuration = 3.0f; // This duration can be adjusted as needed

    [Header("Piece Configuration")]
    [SerializeField] private PieceColor pieceColor = PieceColor.Black; // Default value

    public PieceColor PieceColorValue
    {
        get { return pieceColor; }
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
        // Any other properties to reset can be added here.

        if (rb != null) 
        {
            rb.velocity = Vector3.zero;  // Reset linear velocity.
            rb.angularVelocity = Vector3.zero;  // Reset angular (rotational) velocity.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(pushTimer > 0.0f && other.TryGetComponent<Piece>(out Piece otherPiece) && otherPiece.PieceColorValue != this.PieceColorValue)
        {
            // Kick Interaction Detected
            if(agent != null) 
            {
                agent.AddReward(50f);
                Debug.Log("Successfull Kick Interaction!");
                
                agent.EndEpisode();
            }
        }
    }

    // Update is called once per frame
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
