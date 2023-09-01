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
    public List<Rigidbody> pieceRbList = new List<Rigidbody>();
    // public List<Rigidbody> boardRbList = new List<Rigidbody>();
    public GameObject boardRb = null;
    List<Vector3> m_pieceStartingPosList = new List<Vector3>();
    List<Quaternion> m_pieceStartingRotList = new List<Quaternion>();

    //List of Agents On Platform
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    private ChessSettings m_ChessSettings;


    public SimpleMultiAgentGroup m_BlackAgentGroup;

    public SimpleMultiAgentGroup m_WhiteAgentGroup;

    private Agent blackAgent;
    private Agent whiteAgent;

    public int currentScoreWhite = 0;

    public int currentScoreBlack = 0;

    private int m_ResetTimer;

    public int _whitePiecesLeft = 16;
    public int _blackPiecesLeft = 16;

    [Header("Initial Position Ranges")]
    [SerializeField] private float startXMin = 3f;
    [SerializeField] private float startXMax = 11.5f;
    [SerializeField] private float startYMin = 0f;
    [SerializeField] private float startYMax = 0f;
    [SerializeField] private float startZMin = -6.7f;
    [SerializeField] private float startZMax = -6.7f;

    [SerializeField] private float startZMinAgent2 = 2.0f;
    [SerializeField] private float startZMaxAgent2 = 2.0f;

    void Start()
    {
        m_ChessSettings = FindObjectOfType<ChessSettings>();
        // Initialize TeamManager
        m_BlackAgentGroup = new SimpleMultiAgentGroup();
        m_WhiteAgentGroup = new SimpleMultiAgentGroup();

        foreach (GameObject piece in PiecesList)
        {
            pieceRbList.Add(piece.GetComponent<Rigidbody>());
            m_pieceStartingPosList.Add(new Vector3(piece.transform.position.x, piece.transform.position.y, piece.transform.position.z));
            m_pieceStartingRotList.Add(Quaternion.Euler(piece.transform.rotation.eulerAngles));

        }
        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
            if (item.Agent.team == Team.Black)
            {
                m_BlackAgentGroup.RegisterAgent(item.Agent);
                blackAgent = item.Agent;
            }
            else
            {
                m_WhiteAgentGroup.RegisterAgent(item.Agent);
                whiteAgent = item.Agent;
            }
        }
        //ResetScene();
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
        ObservePiecesLeft();
    }


    public void Resetpieces()
    {

        for (int i = 0; i < PiecesList.Count; i++)
        {
            PiecesList[i].transform.position = m_pieceStartingPosList[i];
            PiecesList[i].transform.rotation = m_pieceStartingRotList[i];
            pieceRbList[i].velocity = Vector3.zero;
            pieceRbList[i].angularVelocity = Vector3.zero;
        }
    }
    //TODO: All pieces for one of the playes are kicked out of the board
    public void GoalTouched(Team scoredTeam)
    {
        // Reward kicking off opponent pieces
        if (scoredTeam == Team.Black)
        {
            // Reward for using own pieces to kick off opponent pieces
            m_BlackAgentGroup.AddGroupReward(.8f - (float)m_ResetTimer / MaxEnvironmentSteps);
            //m_WhiteAgentGroup.AddGroupReward(-.5f);
            currentScoreBlack = currentScoreBlack + 500;
        }
        else
        {
            // Reward for using own pieces to kick off opponent pieces
            m_WhiteAgentGroup.AddGroupReward(.8f - (float)m_ResetTimer / MaxEnvironmentSteps);
            //m_BlackAgentGroup.AddGroupReward(-.5f);
            currentScoreWhite = currentScoreWhite + 500;

        }
        //m_WhiteAgentGroup.EndGroupEpisode();
        //m_BlackAgentGroup.EndGroupEpisode();

        //ResetScene();

    }

    public void ResetAgent(string agent)
    {
        if (agent == "whiteAgent")
        {

            AgentsList[0].Agent.transform.localPosition = new Vector3(
            Random.Range(startXMin, startXMax),
            Random.Range(startYMin, startYMax),
            Random.Range(startZMin, startZMax)
            );

            AgentsList[0].Agent.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            AgentsList[1].Agent.transform.localPosition = new Vector3(
                Random.Range(startXMin, startXMax),
                Random.Range(startYMin, startYMax),
                Random.Range(startZMinAgent2, startZMaxAgent2)
            );

            AgentsList[1].Agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        AgentsList[0].Rb.velocity = Vector3.zero;
        AgentsList[0].Rb.angularVelocity = Vector3.zero;

        AgentsList[1].Rb.velocity = Vector3.zero;
        AgentsList[1].Rb.angularVelocity = Vector3.zero;

    }

    public void ResetScene()
    {

        m_WhiteAgentGroup.EndGroupEpisode();
        m_BlackAgentGroup.EndGroupEpisode();

        m_ResetTimer = 0;

        ResetAgent("whiteAgent");
        ResetAgent("blackAgent");

        //Reset pieces
        Resetpieces();

        _whitePiecesLeft = 16;
        _blackPiecesLeft = 16;

        currentScoreWhite = 0;
        currentScoreBlack = 0;

    }

    public void ObservePiecesLeft(){
        if(_blackPiecesLeft == 0){
            //m_WhiteAgentGroup.AddGroupReward(2f);
            //m_BlackAgentGroup.AddGroupReward(-1f);
            //m_WhiteAgentGroup.AddGroupReward(1f);
            ResetScene();
        }
        if(_whitePiecesLeft == 0){
            //m_BlackAgentGroup.AddGroupReward(2f);
            //m_BlackAgentGroup.AddGroupReward(1f);
            //m_WhiteAgentGroup.AddGroupReward(-1f);
            ResetScene();
        }
    }
}
