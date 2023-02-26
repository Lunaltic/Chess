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
	Color baseColor;
	private void Start()
	{
		baseColor = GetComponent<SpriteRenderer>().color;
		piece = GetComponentInChildren<Piece>();
		coord = new Vector2(transform.position.x, transform.position.y);
	}

	private void OnMouseDown()
	{
		if (GameManager.Instance.status == GameManager.GameStatus.drawingBoard) return;

		// Checking to see that no squares are already selected
		if(GameManager.Instance.selectedSquare == Vector2.zero)
		{
			// If there is no piece on that square, nothing will happens
			if (piece.type == Piece.PieceType.none) UnSelectSquare();

			// Otherwise we will select that square
			SelectSquare();
			return;
		}

		// Now, if there is already a selected square
		else
		{
			if (piece.pieceColor == Piece.PieceColor.white && piece.type != Piece.PieceType.none)
			{
				UnSelectSquare();
				//SelectSquare();
				return;
			}
		}
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

	void SelectSquare()
	{
		GameManager.Instance.selectedSquare = coord;
		GetComponent<SpriteRenderer>().color = new Color(.3f, .3f, .3f, 1);
	}

	void UnSelectSquare()
	{
		GameManager.Instance.selectedSquare = Vector2.zero;
		GetComponent<SpriteRenderer>().color = baseColor;
	}
}
