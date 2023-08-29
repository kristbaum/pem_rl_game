using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class ChessEnvController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public AgentChess Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
    }


    /// <summary>
    /// Max steps before this platform resets
    /// </summary>
    /// <returns></returns>
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    /// <summary>
    /// The area bounds.
    /// </summary>

    /// <summary>
    /// We will be changing the ground material based on success/failue
    /// </summary>

    public List<GameObject> PiecesList = new List<GameObject>();
    [HideInInspector]
    public Rigidbody pieceRb;
    Vector3 m_pieceStartingPos;

    //List of Agents On Platform
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    private ChessSettings m_ChessSettings;


    private SimpleMultiAgentGroup m_BlackAgentGroup;
    private SimpleMultiAgentGroup m_WhiteAgentGroup;

    private int m_ResetTimer;

    void Start()
    {

        m_ChessSettings = FindObjectOfType<ChessSettings>();
        // Initialize TeamManager
        m_BlackAgentGroup = new SimpleMultiAgentGroup();
        m_WhiteAgentGroup = new SimpleMultiAgentGroup();

        foreach (GameObject piece in PiecesList)
        {
            pieceRb = piece.GetComponent<Rigidbody>();
            m_pieceStartingPos = new Vector3(piece.transform.position.x, piece.transform.position.y, piece.transform.position.z);
        }
        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
            if (item.Agent.team == Team.Black)
            {
                m_BlackAgentGroup.RegisterAgent(item.Agent);
            }
            else
            {
                m_WhiteAgentGroup.RegisterAgent(item.Agent);
            }
        }
        ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            m_BlackAgentGroup.GroupEpisodeInterrupted();
            m_WhiteAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }


    public void Resetpiece()
    {
        var randomPosX = Random.Range(-2.5f, 2.5f);
        var randomPosZ = Random.Range(-2.5f, 2.5f);


        foreach (GameObject piece in PiecesList)
        {

            piece.transform.position = m_pieceStartingPos + new Vector3(randomPosX, 0f, randomPosZ);
            pieceRb.velocity = Vector3.zero;
            pieceRb.angularVelocity = Vector3.zero;
        }
    }

    public void GoalTouched(Team scoredTeam)
    {
        if (scoredTeam == Team.Black)
        {
            m_BlackAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_WhiteAgentGroup.AddGroupReward(-1);
        }
        else
        {
            m_WhiteAgentGroup.AddGroupReward(1 - (float)m_ResetTimer / MaxEnvironmentSteps);
            m_BlackAgentGroup.AddGroupReward(-1);
        }
        m_WhiteAgentGroup.EndGroupEpisode();
        m_BlackAgentGroup.EndGroupEpisode();
        Debug.Log("Goal Touched");
        ResetScene();

    }


    public void ResetScene()
    {
        m_ResetTimer = 0;

        //Reset Agents
        foreach (var item in AgentsList)
        {
            var randomPosX = Random.Range(-5f, 5f);
            var newStartPos = item.Agent.initialPos + new Vector3(randomPosX, 0f, 0f);
            var rot = item.Agent.rotSign * Random.Range(80.0f, 100.0f);
            var newRot = Quaternion.Euler(0, rot, 0);
            item.Agent.transform.SetPositionAndRotation(newStartPos, newRot);

            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
        }

        //Reset piece
        Resetpiece();
    }
}
