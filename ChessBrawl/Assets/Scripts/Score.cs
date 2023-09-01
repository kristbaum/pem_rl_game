using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public TextMeshProUGUI _scoreBlackText;
    public TextMeshProUGUI _scoreWhiteText;

    public ChessEnvController _chess = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _scoreBlackText.text = _chess.currentScoreBlack.ToString();

        _scoreWhiteText.text = _chess.currentScoreWhite.ToString();
    }
}
