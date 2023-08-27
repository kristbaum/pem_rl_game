using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceColor
    {
        Black,
        White
    }

    [Header("Piece Configuration")]
    [SerializeField] private PieceColor pieceColor = PieceColor.Black; // Default value

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
