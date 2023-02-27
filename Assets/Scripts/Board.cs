using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Board : MonoBehaviour
{
	public float delay = .02f;
	public GameObject square;
	public Color whiteColor = Color.white;
	public Color blackColor = Color.black;
	public static Board Instance;
	[Header("Black Pieces")]
	public Sprite blackPawn;
	public Sprite blackKnight;
	public Sprite blackBishop;
	public Sprite blackRook;
	public Sprite blackQueen;
	public Sprite blackKing;

	[Space]
	[Header("White Pieces")]
	public Sprite whitePawn;
	public Sprite whiteKnight;
	public Sprite whiteBishop;
	public Sprite whiteRook;
	public Sprite whiteQueen;
	public Sprite whiteKing;

	void Awake()
    {
		Instance = this;

		StartCoroutine(DrawBoardWithDelay());
		StartCoroutine(StartDrawingPieces());
		Invoke("ReadyToPlay", delay * 128);
    }


	public IEnumerator DrawBoardWithDelay()
	{
		for (int x = 1; x <= 8; x++)
		{
			for (int y = 1; y <= 8; y++)
			{
				square.GetComponent<Square>().index = (y - 1) * 8 + (x - 1);
				square.GetComponent<SpriteRenderer>().color = (x + y) % 2 == 0 ? blackColor : whiteColor;
				Instantiate(square, new Vector3(x, y, 0), Quaternion.identity, transform);

				yield return new WaitForSeconds(delay);
			}
		}
	}

	public IEnumerator StartDrawingPieces()
	{
		yield return new WaitForSeconds(delay * 64);
		StartCoroutine(DrawStartingPieces());
	}
	public IEnumerator DrawStartingPieces()
	{
		foreach (Transform child in transform)
		{
			//pawns
			Square childSquare = child.GetComponent<Square>();
			if (childSquare.index >= 8 && childSquare.index <= 15) { childSquare.piece.type = Piece.PieceType.pawn; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index >= 48 && childSquare.index <= 55) { childSquare.piece.type = Piece.PieceType.pawn; childSquare.piece.pieceColor = Piece.PieceColor.black; }

			if (childSquare.index == 0 || childSquare.index == 7) { childSquare.piece.type = Piece.PieceType.rook; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index == 56 || childSquare.index == 63) { childSquare.piece.type = Piece.PieceType.rook; childSquare.piece.pieceColor = Piece.PieceColor.black; }

			if (childSquare.index == 1 || childSquare.index == 6) { childSquare.piece.type = Piece.PieceType.knight; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index == 57 || childSquare.index == 62) { childSquare.piece.type = Piece.PieceType.knight; childSquare.piece.pieceColor = Piece.PieceColor.black; }

			if (childSquare.index == 2 || childSquare.index == 5) { childSquare.piece.type = Piece.PieceType.bishop; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index == 58 || childSquare.index == 61) { childSquare.piece.type = Piece.PieceType.bishop; childSquare.piece.pieceColor = Piece.PieceColor.black; }

			if (childSquare.index == 3) { childSquare.piece.type = Piece.PieceType.queen; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index == 59) { childSquare.piece.type = Piece.PieceType.queen; childSquare.piece.pieceColor = Piece.PieceColor.black; }


			if (childSquare.index == 4) { childSquare.piece.type = Piece.PieceType.king; childSquare.piece.pieceColor = Piece.PieceColor.white; }
			if (childSquare.index == 60) { childSquare.piece.type = Piece.PieceType.king; childSquare.piece.pieceColor = Piece.PieceColor.black; }
			yield return new WaitForSeconds(delay/1.75f);
		}
	}

	public void ReadyToPlay()
	{
		GameManager.Instance.status = GameManager.GameStatus.readyToPlay;
		print("ready");
	}

	public void ResetPieces()
	{
		foreach (Transform child in transform)
		{
			//pawns
			Square childSquare = child.GetComponent<Square>();
			childSquare.piece.type = Piece.PieceType.none;
			childSquare.piece.pieceColor = Piece.PieceColor.white;

			StartCoroutine(DrawStartingPieces());

		}
	}
}
