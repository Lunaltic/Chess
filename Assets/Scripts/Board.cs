using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Board : MonoBehaviour
{
	[Header("Settings")]
	public float delay = .02f;
	public GameObject square;
	public Color whiteColor = Color.white;
	public Color blackColor = Color.black;
	public Color lastMoveColor = Color.green;
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

	public AudioClip moveSFX;
	public AudioClip captureSFX;

	[HideInInspector] public Square[] lastSquaresToMove;

	public List<Square> Squares = new List<Square>();

	void Awake()
    {
		Instance = this;

		StartCoroutine(DrawBoardWithDelay());
		StartCoroutine(StartDrawingPieces());
		Invoke("ReadyToPlay", delay * 128);

    }

	public void LoadPosition(string fen)
	{
		var pieceSymbol = new Dictionary<char, Piece.PieceType>()
		{
			['k'] = Piece.PieceType.king,
			['p'] = Piece.PieceType.pawn,
			['n'] = Piece.PieceType.knight,
			['r'] = Piece.PieceType.rook,
			['q'] = Piece.PieceType.queen,
			['b'] = Piece.PieceType.bishop
		};

		string fenBoard = fen.Split(' ')[0];
		int x = 0, y = 8;

		foreach (char symbol in fenBoard)
		{
			if(symbol == '/')
			{
				x = 0;
				y--;
			}
			else
			{
				if (char.IsDigit(symbol))
				{
					x += (int)char.GetNumericValue(symbol);
				}
				else
				{
					Piece.PieceColor pColor = (char.IsUpper(symbol)) ? Piece.PieceColor.white : Piece.PieceColor.black;
					Piece.PieceType pType = (pieceSymbol[char.ToLower(symbol)]);
					foreach (Transform child in transform)
					{
						
						Square childSquare = child.GetComponent<Square>();
						

						childSquare.GetComponentInChildren<Piece>().pieceColor = pColor;
						childSquare.GetComponentInChildren<Piece>().type = pType;
					}
					x++;
				}
			}
		}
	}
	public IEnumerator DrawBoardWithDelay()
	{
		for (int x = 1; x <= 8; x++)
		{
			for (int y = 1; y <= 8; y++)
			{
				square.GetComponent<Square>().index = (y - 1) * 8 + (x - 1);
				square.GetComponent<SpriteRenderer>().color = (x + y) % 2 == 0 ? blackColor : whiteColor;
				square.GetComponent<Square>().baseColor = (x + y) % 2 == 0 ? blackColor : whiteColor;
				Instantiate(square, new Vector3(x, y, 0), Quaternion.identity, transform);

				Squares.Add(square.GetComponent<Square>());
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
		//LoadPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
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

	public void UpdateLastSquares()
	{
		lastSquaresToMove[0].lastMove = true;
		lastSquaresToMove[1].lastMove = true;
	}

	public void RemoveLastSquares()
	{
		if(lastSquaresToMove.Length >= 2)
		{
			lastSquaresToMove[0].lastMove = false;
			lastSquaresToMove[1].lastMove = false;
		}
	}

}
