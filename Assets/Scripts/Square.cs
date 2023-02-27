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
	bool isValid = false;

	Color baseColor;
	private void Start()
	{
		baseColor = GetComponent<SpriteRenderer>().color;
		piece = GetComponentInChildren<Piece>();
		coord = new Vector2(transform.position.x, transform.position.y);

		
	}

	private void Update()
	{
		if (!highlighted)
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

		piece.type = GameManager.Instance.type;
		piece.pieceColor = GameManager.Instance.pieceColor;

		GameManager.Instance.selectedSquare.piece.type = Piece.PieceType.none;
		UnSelectSquare();

		// This code will change the player's turn
		if(GameManager.Instance.pieceColor == Piece.PieceColor.white) { GameManager.Instance.pieceColor = Piece.PieceColor.black; } else { GameManager.Instance.pieceColor = Piece.PieceColor.white; }
	}

	void UpdateNotation()
	{
		int movesCounter = GameManager.Instance.moves.Length;
		Array.Resize(ref GameManager.Instance.moves, movesCounter + 1);
		GameManager.Instance.moves[movesCounter] = coord.ToString();
		GameManager.Instance.newMove();
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
				GetComponent<SpriteRenderer>().color = Color.red;
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
		if (GameManager.Instance.type == Piece.PieceType.king)
		{

			if(square.index == index + 8 || square.index == index - 8 || square.index == index + 1 || square.index == index - 1
				|| square.index == index + 9 || square.index == index - 9 || square.index == index + 7 || square.index == index - 7)
			{
				MovePiece();
			}
		}
		#endregion

		#region Knight
		if (GameManager.Instance.type == Piece.PieceType.knight)
		{

			if (square.index == index + 17 || square.index == index - 17 || square.index == index + 15 || square.index == index - 15
				|| square.index == index + 10 || square.index == index - 10 || square.index == index + 6 || square.index == index - 6)
			{
				MovePiece();
			}
		}
		#endregion
	}

}



