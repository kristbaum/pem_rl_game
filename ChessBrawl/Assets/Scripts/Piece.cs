using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public ChessEnvController envController;
    public string whiteGoalTag; //will be used to check if collided with white goal
    public string blackGoalTag; //will be used to check if collided with black goal

    private new string tag;

    void Start()
    {

        envController = area.GetComponent<ChessEnvController>();
        tag = gameObject.tag;
    }

    void OnCollisionEnter(Collision col)
    {
        if (gameObject.CompareTag(tag))
        {

            if (col.gameObject.CompareTag(whiteGoalTag)) //white piece touched white goal
            {
                Debug.Log("White scored!!!");
                envController.GoalTouched(Team.White);
            }

        }

        if (gameObject.CompareTag(tag))
        {
            if (col.gameObject.CompareTag(blackGoalTag)) //black piece touched black goal
            {
                Debug.Log("Black scored!!!");
                envController.GoalTouched(Team.Black);
            }

        }
    }

        void OnTriggerExit(Collider col)
    {


  if (gameObject.CompareTag(tag))
        {
            
        if(tag == "whitePiece" && col.gameObject.CompareTag("outsideBorder")){
            envController._whitePiecesLeft = envController._whitePiecesLeft-1;
        }
        if(tag == "blackPiece"&& col.gameObject.CompareTag("outsideBorder")){
            envController._blackPiecesLeft = envController._blackPiecesLeft-1;
        }
    }
}
}
