using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    [Header("Agent Configuration")]
    public List<MoveAgent> Agents;

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
        // Sync the AllAgents list in MoveAgent with the Agents in GameManager
        MoveAgent.AllAgents = Agents;
    }
}
