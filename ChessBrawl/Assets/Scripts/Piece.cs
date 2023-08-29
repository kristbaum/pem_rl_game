using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public ChessEnvController envController;
    public string whiteGoalTag; //will be used to check if collided with white goal
    public string blackGoalTag; //will be used to check if collided with black goal

    void Start()
    {
        envController = area.GetComponent<ChessEnvController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(whiteGoalTag)) //piece touched white goal
        {
            Debug.Log("Team black??");
            Debug.Log(Team.Black);
            envController.GoalTouched(Team.Black);
        }
        if (col.gameObject.CompareTag(blackGoalTag)) //piece touched black goal
        {
            envController.GoalTouched(Team.White);
        }
    }
}
