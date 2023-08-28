using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton pattern implementation
    public static GameManager Instance;

    [Header("Agent Configuration")]

    //Agent list
    public List<MoveAgent> Agents = new List<MoveAgent>();

    public float CPWall = 0f;
    public float CPTouchingAgent = 0f;
    public float CPTouchingOpponentPiece = 0f;
    public float CPKickingOffOwnPiece = 0f;

    public float CRTouchingOwnPiece = 0f;
    public float CRKickMotion = 0f;
    public float CRKickingOffOpponentPiece = 0f;

    public int currentScore = 0;
    private void Awake()
        {
            // Singleton pattern initialization
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

            // Optionally, if you want this manager to persist across scenes
            // DontDestroyOnLoad(this.gameObject);
        }

        // Add any other GameManager related functions and logic below
}
