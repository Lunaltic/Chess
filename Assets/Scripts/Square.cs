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
		piece.type = GameManager.Instance.type;
		piece.pieceColor = GameManager.Instance.pieceColor;

		GameManager.Instance.selectedSquare.piece.type = Piece.PieceType.none;
		UnSelectSquare();

		// This code will change the player's turn
		if(GameManager.Instance.pieceColor == Piece.PieceColor.white) { GameManager.Instance.pieceColor = Piece.PieceColor.black; } else { GameManager.Instance.pieceColor = Piece.PieceColor.white; }
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

	public void Validate()
	{
		if (GameManager.Instance.type == Piece.PieceType.pawn)
		{
			if (GameManager.Instance.selectedSquare.index == index - 8 && GameManager.Instance.pieceColor == Piece.PieceColor.white)
			{
				MovePiece();
			}

			else if (GameManager.Instance.selectedSquare.index == index + 8 && GameManager.Instance.pieceColor == Piece.PieceColor.black)
			{
				MovePiece();
			}
		}
	}

}



