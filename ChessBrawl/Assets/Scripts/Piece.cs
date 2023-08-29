using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public ChessEnvController envController;
    public string whiteGoalTag; //will be used to check if collided with white goal
    public string blackGoalTag; //will be used to check if collided with black goal

    public string whitePiece;
    public string blackPiece;

    void Start()
    {
        envController = area.GetComponent<ChessEnvController>();
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag(whitePiece))
        {
            Debug.Log("WhitePieceTouched");

            if (col.gameObject.CompareTag(whiteGoalTag)) //white piece touched white goal
        {
                Debug.Log("White scored!!!");
                envController.GoalTouched(Team.White);
            }

        }

        if (col.gameObject.CompareTag(blackPiece))
        {
            if (col.gameObject.CompareTag(blackGoalTag)) //black piece touched black goal
            {
                Debug.Log("Black scored!!!");
                envController.GoalTouched(Team.Black);
            }

        }


    }
}
