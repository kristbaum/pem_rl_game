using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
        private void OnTriggerEnter(Collider other)
    {
        Piece triggeredPiece = other.gameObject.GetComponent<Piece>();

        if (triggeredPiece != null)
        {
            int oppositeColorValue = GetOppositeColorValue((int)triggeredPiece.PieceColorValue);

            foreach (var agent in MoveAgent.AllAgents)
            {
                // Penalize all agents with the same color as the piece that collided
                if ((int)agent.AgentColorValue == (int)triggeredPiece.PieceColorValue)
                {
                    float penalty = triggeredPiece.PieceValue;
                    agent.AddReward(-penalty); // Negative reward
                    Debug.Log("Penalized agent with color: " + agent.AgentColorValue.ToString() + " for losing piece " + triggeredPiece.gameObject.name + " with value " + triggeredPiece.PieceValue.ToString());
                }
                // Reward the agent with the opposite color
                else if ((int)agent.AgentColorValue == oppositeColorValue)
                {
                    float reward = triggeredPiece.PieceValue;
                    agent.AddReward(reward); // Positive reward
                    Debug.Log("Rewarded agent with color: " + agent.AgentColorValue.ToString() + " for opponent losing piece " + triggeredPiece.gameObject.name + " with value " + triggeredPiece.PieceValue.ToString());
                }
            }
        }
    }

    // Define this function based on your color system. This example assumes only two colors (0 and 1).
    private int GetOppositeColorValue(int colorValue)
    {
        return colorValue == 0 ? 1 : 0;
    }

}