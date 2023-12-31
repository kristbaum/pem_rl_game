using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public ChessEnvController envController;
    private string whiteGoalTag = "whiteWallInvisible"; //will be used to check if collided with white goal
    private string blackGoalTag = "blackWallInvisible"; //will be used to check if collided with black goal

    private new string tag;

    void Start()
    {

        envController = area.GetComponent<ChessEnvController>();
        tag = gameObject.tag;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("whiteTarget") && tag == "blackPiece") //black piece touched white goal
        {
            envController.GoalTouched(Team.White);
            Debug.Log("black piece touched white goal");
        }

        if (col.gameObject.CompareTag("blackTarget") && tag == "whitePiece") //white piece touched black goal
        {
            envController.GoalTouched(Team.Black);
            Debug.Log("white piece touched black goal");
        }

    }

    void OnTriggerExit(Collider col)
    {
        if (tag == "whitePiece" && col.gameObject.CompareTag("outsideBorder"))
        {
            envController._whitePiecesLeft = envController._whitePiecesLeft - 1;
            envController.m_WhiteAgentGroup.AddGroupReward(-0.3f);
        }
        if (tag == "blackPiece" && col.gameObject.CompareTag("outsideBorder"))
        {
            envController._blackPiecesLeft = envController._blackPiecesLeft - 1;
            envController.m_BlackAgentGroup.AddGroupReward(-0.3f);
        }
    }
}
