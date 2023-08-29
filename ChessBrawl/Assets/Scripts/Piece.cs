using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject area;
    [HideInInspector]
    public ChessEnvController envController;
    public string whiteGoalTag; //will be used to check if collided with white goal
    public string blackGoalTag; //will be used to check if collided with black goal

    private new string tag;

    //private Vector3 initialPosition;
    //private Quaternion initialRotation;

    //private Rigidbody rb;

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    void Start()
    {
        //initialPosition = transform.position;
        //initialRotation = transform.rotation;

        envController = area.GetComponent<ChessEnvController>();
        tag = gameObject.tag;
    }

    //public void ResetPiece()
    //{
    //    transform.position = initialPosition;
    //    transform.rotation = initialRotation;

    //    if (rb != null)
    //    {
    //        rb.velocity = Vector3.zero;  // Reset linear velocity.
    //        rb.angularVelocity = Vector3.zero;  // Reset angular (rotational) velocity.
    //    }
    //}

    void OnCollisionEnter(Collision col)
    {

        if (gameObject.CompareTag(tag))
        {
            Debug.Log("WhitePieceTouched");

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
}
