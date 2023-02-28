using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
	public Vector2 coord;
	public Piece piece;
	public int index;
	bool selected = false;
	bool highlighted = false;
	[HideInInspector] public bool lastMove = false;

	[HideInInspector] public Color baseColor;

	string coordA;
	private void Start()
	{
		piece = GetComponentInChildren<Piece>();
		coord = new Vector2(transform.position.x, transform.position.y);

		switch (coord.x)
		{
			case 1:
				coordA = "a";
				break;
			case 2:
				coordA = "b";
				break;
			case 3:
				coordA = "c";
				break;
			case 4:
				coordA = "d";
				break;
			case 5:
				coordA = "e";
				break;
			case 6:
				coordA = "f";
				break;
			case 7:
				coordA = "g";
				break;
			case 8:
				coordA = "h";
				break;
		}
	}

	private void Update()
	{
		if (!highlighted && !lastMove)
		{
			if (GameManager.Instance.selectedSquare != this)
			{
				GetComponent<SpriteRenderer>().color = baseColor;
			}
			else
			{
				GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
			}
		}

		if (lastMove)
		{
			GetComponent<SpriteRenderer>().color = Board.Instance.lastMoveColor;
		}


	}

	private void OnMouseDown()
	{
		if (GameManager.Instance.status == GameManager.GameStatus.drawingBoard) return;

		// Checking to see that no squares are already selected
		if(GameManager.Instance.selectedSquare == null)
		{
			// If there is no piece on that square, nothing will happens
			if (piece.type == Piece.PieceType.none) { UnSelectSquare(); return; }

			// Otherwise we will select that square
			else { if (piece.pieceColor == GameManager.Instance.pieceColor) { SelectSquare(); return; } }
		}


		// Now, if there is already a selected square
		if(GameManager.Instance.selectedSquare != null)
		{
			//  The selected piece will move it to the following square
			if (piece.type == Piece.PieceType.none || piece.pieceColor != GameManager.Instance.pieceColor)
			{
				Validate();
			}


			// Checking if the player wants to unselect or select another piece
			if (GameManager.Instance.selectedSquare == this) { UnSelectSquare(); return; }
			else { if(piece.pieceColor == GameManager.Instance.pieceColor) { SelectSquare(); return; } }
		}

	}

	void MovePiece()
	{
		UpdateNotation();
		Board.Instance.RemoveLastSquares();
		Board.Instance.lastSquaresToMove = new Square[]{ this, GameManager.Instance.selectedSquare };
		Board.Instance.UpdateLastSquares();

		piece.type = GameManager.Instance.type;
		piece.pieceColor = GameManager.Instance.pieceColor;

		GameManager.Instance.selectedSquare.piece.type = Piece.PieceType.none;
		UnSelectSquare();
		Board.Instance.GetComponent<AudioSource>().Play();

		// This code will change the player's turn
		if(GameManager.Instance.pieceColor == Piece.PieceColor.white) { GameManager.Instance.pieceColor = Piece.PieceColor.black; } else { GameManager.Instance.pieceColor = Piece.PieceColor.white; }
	}

	void UpdateNotation()
	{
		int movesCounter = GameManager.Instance.moves.Length;
		Array.Resize(ref GameManager.Instance.moves, movesCounter + 1);
		switch (GameManager.Instance.type)
		{
			case Piece.PieceType.none:
				GameManager.Instance.moves[movesCounter] = coordA + coord.y;
				break;
			case Piece.PieceType.pawn:
				GameManager.Instance.moves[movesCounter] = coordA + coord.y;
				break;
			case Piece.PieceType.knight:
				GameManager.Instance.moves[movesCounter] = "N"+coordA + coord.y;
				break;
			case Piece.PieceType.bishop:
				GameManager.Instance.moves[movesCounter] = "B"+coordA + coord.y;
				break;
			case Piece.PieceType.rook:
				GameManager.Instance.moves[movesCounter] = "R"+coordA + coord.y;
				break;
			case Piece.PieceType.queen:
				GameManager.Instance.moves[movesCounter] = "Q"+coordA + coord.y;
				break;
			case Piece.PieceType.king:
				GameManager.Instance.moves[movesCounter] = "K"+coordA + coord.y;
				break;
		}
		GameManager.Instance.newMove();

		UIManager.Instance.NewNotation();
	}


	void SelectSquare()
	{
		GameManager.Instance.selectedSquare = this;
		GameManager.Instance.type = piece.type;
		GameManager.Instance.pieceColor = piece.pieceColor;
	}

	void UnSelectSquare()
	{
		GameManager.Instance.type = Piece.PieceType.none;
		GameManager.Instance.selectedSquare = null;
	}

	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(1) && GameManager.Instance.status != GameManager.GameStatus.drawingBoard)
		{
			if (!highlighted)
			{
				highlighted = true;
			}
			else
			{
				highlighted = false;
				GetComponent<SpriteRenderer>().color = baseColor;
			}
		}
	}

	// This method is used to validate a move, to check if its possible or not
	public void Validate()
	{
		Square square = GameManager.Instance.selectedSquare;
		Piece.PieceType pType = GameManager.Instance.type;
		Piece.PieceColor pColor = GameManager.Instance.pieceColor;

		#region Pawn
		if (pType == Piece.PieceType.pawn)
		{
			// WHITE PAWN

			if(pColor == Piece.PieceColor.white)
			{
				// Checking if the pawn has moved before
				if (square.coord.y == 2  && (square.index == index - 8 || square.index == index - 16))
				{
					MovePiece();
				}

				// If not, check if the pawn move is legal
				else
				{
					if(square.index == index - 8) { MovePiece(); }
				}
			}

			// BLACK PAWN

			if (pColor == Piece.PieceColor.black)
			{
				// Checking if the pawn has moved before
				if (square.coord.y == 7 &&
					(square.index == index + 8 ||
					square.index == index + 16))
				{
					MovePiece();
				}

				// If not, check if the pawn move is legal
				else
				{
					if (square.index == index + 8) { MovePiece(); }
				}
			}

		}
		#endregion

		#region King
		if (pType == Piece.PieceType.king)
		{

			if(square.index == index + 8 || square.index == index - 8 || square.index == index + 1 || square.index == index - 1
				|| square.index == index + 9 || square.index == index - 9 || square.index == index + 7 || square.index == index - 7)
			{
				MovePiece();
			}
		}
		#endregion

		#region Knight
		if (pType == Piece.PieceType.knight)
		{

			if (square.index == index + 17 || square.index == index - 17 || square.index == index + 15 || square.index == index - 15
				|| square.index == index + 10 || square.index == index - 10 || square.index == index + 6 || square.index == index - 6)
			{
				MovePiece();
			}
		}
		#endregion

		#region Rook
		if(pType == Piece.PieceType.rook)
		{
			// Check if the rook is moving only on its own file/collumn
			if(square.coord.x == coord.x || square.coord.y == coord.y)
			{
				// Will look if there is a piece between the initial square and the final one
				// If there is a piece on the middle, then the move won't be played

				// ERRO QUE TA DANDO: a torre detecta peças que estão antes da casa de inicio, ou seja, que não eram pra ser consideradas
				// Tentei verificar quando a torre está subindo, mas não foi
				foreach (Transform squaresToAnalyze in Board.Instance.transform)
				{
					Square currentSquare = squaresToAnalyze.GetComponent<Square>();
					float deltaX = Mathf.Abs(coord.x - square.coord.x);
					float deltaY = Mathf.Abs(coord.y - square.coord.y);

					if (currentSquare.coord.x == coord.x)
					{
						currentSquare.highlighted = true;
						if ((coord.y - square.coord.y < 0 && currentSquare.coord.y >= deltaY && currentSquare.coord.y != square.coord.y) ||
							(coord.y - square.coord.y > 0 && currentSquare.coord.y <= deltaY && currentSquare.coord.y != square.coord.y))
						{
							if (currentSquare.piece.type != Piece.PieceType.none) { print(currentSquare.coord); return; }
						}
					}

					if (currentSquare.coord.y == coord.y)
					{
						if ((coord.x - square.coord.x > 0 && currentSquare.coord.x <= deltaX && currentSquare.coord.x != square.coord.x) ||
							(coord.x - square.coord.x < 0 && currentSquare.coord.x >= deltaX && currentSquare.coord.x != square.coord.x))
						{
							if (currentSquare.piece.type != Piece.PieceType.none) { return; }
						}
					}
				}

				// No pieces between squares detected, playing the move
				MovePiece();
			}
		}
		#endregion

		
	}

}



