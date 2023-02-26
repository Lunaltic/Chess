using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStatus status;
    public static GameManager Instance;
    [Header("Current Square:")]
    public Square selectedSquare;
    [HideInInspector] public Piece selectedPiece;
    public Piece.PieceType type;
    public Piece.PieceColor pieceColor;
    public Vector2 targetSquare;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedPiece != null)
		{
            type = selectedSquare.piece.type;
            pieceColor = selectedSquare.piece.pieceColor;
        }

    }

    public enum GameStatus
	{
        drawingBoard, readyToPlay, playing, gameOver
	}
}
