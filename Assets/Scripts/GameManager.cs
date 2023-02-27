using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStatus status;
    public static GameManager Instance;
    [Header("Current Square:")]
    public Square selectedSquare;
    public Piece.PieceType type;
    public Piece.PieceColor pieceColor;
    public Vector2 targetSquare;
    public string[] moves;
    public string gameNotation;

    


    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       // print(selectedSquare.piece + "\n" + selectedSquare.piece.type + " " + selectedSquare.piece.pieceColor);

    }

    public void newMove()
	{
		for (int i = 0; i < moves.Length; i++)
		{
            gameNotation = gameNotation + "/" + moves[i];
		}
	}

    public enum GameStatus
	{
        drawingBoard, readyToPlay, playing, gameOver
	}
}
