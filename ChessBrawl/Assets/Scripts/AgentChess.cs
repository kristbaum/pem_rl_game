using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public enum Team
{
    Black = 0,
    White = 1
}

public class AgentChess : Agent
{
    // Note that that the detectable tags are different for the Black and White teams. The order is
    // * piece
    // * own goal
    // * opposing goal
    // * wall
    // * own teammate
    // * opposing player

    public enum Position
    {
        Striker
    }

    private string agentTag;

    [HideInInspector]
    public Team team;
    float m_KickPower;
    // The coefficient for the reward for colliding with a piece. Set using curriculum.
    float m_PieceTouch;
    public Position position;

    const float k_Power = 200f;
    float m_Existential;
    float m_LateralSpeed;
    float m_ForwardSpeed;


    [HideInInspector]
    public Rigidbody agentRb;
    ChessSettings m_ChessSettings;
    BehaviorParameters m_BehaviorParameters;
    public Vector3 initialPos;
    public float rotSign;

    EnvironmentParameters m_ResetParams;

    void Start()
    {

        agentTag = gameObject.tag;
    }


    public override void Initialize()
    {
        ChessEnvController envController = GetComponentInParent<ChessEnvController>();
        if (envController != null)
        {
            m_Existential = 1f / envController.MaxEnvironmentSteps;
        }
        else
        {
            m_Existential = 1f / MaxStep;
        }

        m_BehaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (m_BehaviorParameters.TeamId == (int)Team.Black)
        {
            team = Team.Black;
            initialPos = new Vector3(transform.position.x - 5f, .5f, transform.position.z);
            rotSign = 1f;
        }
        else
        {
            team = Team.White;
            initialPos = new Vector3(transform.position.x + 5f, .5f, transform.position.z);
            rotSign = -1f;
        }
        if (position == Position.Striker)
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 0.7f;
        }
        else
        {
            m_LateralSpeed = 0.3f;
            m_ForwardSpeed = 1.0f;
        }
        m_ChessSettings = FindObjectOfType<ChessSettings>();
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        m_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                m_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        agentRb.AddForce(dirToGo * m_ChessSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)

    {

        if (position == Position.Striker)
        {
            // Existential penalty for standing on the same position
            //AddReward(-m_Existential);
            AddReward(-.1f);
        }
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        //forward
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 2;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }
        //right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }
    /// <summary>
    /// Used to provide a "kick" to the piece.
    /// </summary>
    void OnCollisionEnter(Collision c)
    {
        var force = k_Power * m_KickPower;
        //rewarding touching own pieces 
        if (team.ToString() == "White" && c.gameObject.CompareTag("whitePiece"))
        {
            AddReward(.3f * 1);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }
        //rewarding touching own pieces 
        if (team.ToString() == "Black" && c.gameObject.CompareTag("blackPiece"))
        {
            AddReward(.3f * 1);
            var dir = c.contacts[0].point - transform.position;
            dir = dir.normalized;
            c.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
        }

        //penalise touching opponent pieces 
        if (team.ToString() == "White" && c.gameObject.CompareTag("blackPiece"))
        {
            AddReward(-1f);
        }
        //penalise touching opponent pieces 
        if (team.ToString() == "Black" && c.gameObject.CompareTag("whitePiece"))
        {
            AddReward(-1f);
        }

        if (c.gameObject.CompareTag("blackWallInvisible"))
        {
            // Prevent agent from falling of
            AddReward(-1f);
        }

        if (c.gameObject.CompareTag("whiteWallInvisible"))
        {
            // Prevent agent from falling of
            AddReward(-1f);
        }

                //rewarding touching own pieces 
        if (team.ToString() == "Black" && c.gameObject.CompareTag("whiteAgent"))
        {
            AddReward(-1f);
        }
        if (team.ToString() == "White" && c.gameObject.CompareTag("blackAgent"))
        {
            AddReward(-1f);
        }


    }

    public override void OnEpisodeBegin()
    {
        //m_PieceTouch = m_ResetParams.GetWithDefault("piece_touch", 0);
    }

}
