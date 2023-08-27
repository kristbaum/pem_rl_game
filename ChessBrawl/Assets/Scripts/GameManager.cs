using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    [Header("Agent Configuration")]
    public List<MoveAgent> Agents;

    private void Awake()
    {
        // Sync the AllAgents list in MoveAgent with the Agents in GameManager
        MoveAgent.AllAgents = Agents;
    }
}
