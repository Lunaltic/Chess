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

		if (piece.type != Piece.PieceType.none)
		{
			Board.Instance.GetComponent<AudioSource>().PlayOneShot(Board.Instance.captureSFX);
		}
		else
		{
			Board.Instance.GetComponent<AudioSource>().PlayOneShot(Board.Instance.moveSFX);
		}

		piece.type = GameManager.Instance.type;
		piece.pieceColor = GameManager.Instance.pieceColor;

		GameManager.Instance.selectedSquare.piece.type = Piece.PieceType.none;
		UnSelectSquare();


		

		// This code will change the player's turn
		if(GameManager.Instance.pieceColor == Piece.PieceColor.white) { GameManager.Instance.pieceColor = Piece.PieceColor.black; } else { GameManager.Instance.pieceColor = Piece.PieceColor.white; }
	}

	string updatedNotation;
	void UpdateNotation()
	{
		int movesCounter = GameManager.Instance.moves.Length;
		Array.Resize(ref GameManager.Instance.moves, movesCounter + 1);

		if(GameManager.Instance.selectedSquare.piece.type == Piece.PieceType.pawn && piece.type != Piece.PieceType.none) { updatedNotation = GameManager.Instance.selectedSquare.coordA + "x" + coordA + coord.y; }
		else if(piece.type != Piece.PieceType.none && GameManager.Instance.selectedSquare.piece.type != Piece.PieceType.pawn) { updatedNotation = "x" + coordA + coord.y; }
		else { updatedNotation = coordA + coord.y; }
		

		switch (GameManager.Instance.type)
		{
			case Piece.PieceType.none:
				GameManager.Instance.moves[movesCounter] = updatedNotation;
				break;
			case Piece.PieceType.pawn:
				GameManager.Instance.moves[movesCounter] = updatedNotation;
				break;
			case Piece.PieceType.knight:
				GameManager.Instance.moves[movesCounter] = "N"+ updatedNotation;
				break;
			case Piece.PieceType.bishop:
				GameManager.Instance.moves[movesCounter] = "B"+ updatedNotation;
				break;
			case Piece.PieceType.rook:
				GameManager.Instance.moves[movesCounter] = "R"+ updatedNotation;
				break;
			case Piece.PieceType.queen:
				GameManager.Instance.moves[movesCounter] = "Q"+ updatedNotation;
				break;
			case Piece.PieceType.king:
				GameManager.Instance.moves[movesCounter] = "K"+ updatedNotation;
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
				if (square.coord.y == 2  && (square.index == index - 8 || square.index == index - 16) && piece.type == Piece.PieceType.none)
				{
					MovePiece();
				}

				// If not, check if the pawn move is legal
				else
				{
					if(square.index == index - 8 && piece.type == Piece.PieceType.none) { MovePiece(); }
				}


				if (square.index == index - 9 || square.index == index - 7)
				{
					if (piece.pieceColor == Piece.PieceColor.black && piece.type != Piece.PieceType.none)
					{
						MovePiece();
					}
				}
			}

			// BLACK PAWN

			if (pColor == Piece.PieceColor.black)
			{
				// Checking if the pawn has moved before
				if (square.coord.y == 7 &&
					(square.index == index + 8 ||
					square.index == index + 16) && piece.type == Piece.PieceType.none)
				{
					MovePiece();
				}

				// If not, check if the pawn move is legal
				else
				{
					if (square.index == index + 8 && piece.type == Piece.PieceType.none) { MovePiece(); }
				}

				if(square.index == index + 9 || square.index == index + 7)
				{
					if(piece.pieceColor == Piece.PieceColor.white && piece.type != Piece.PieceType.none)
					{
						MovePiece();
					}
					
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
				foreach (Transform listSquare in Board.Instance.transform)
				{
					Square current = listSquare.GetComponent<Square>();
					// Checking the file of the rook
					if (current.coord.y == coord.y)
					{
						// Moving to the right
						if (coord.x > square.coord.x)
						{
							// Checking between final and initial square to see if there is a piece there
							if(current.coord.x > square.coord.x && current.coord.x < coord.x && current.piece.type != Piece.PieceType.none)
							{
								return;
							}

						}

						// Moving to the Left
						if (coord.x < square.coord.x)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.x < square.coord.x && current.coord.x > coord.x && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}

					// ------------------------------------------------------------------

					// Checking the file of the rook
					if (current.coord.x == coord.x)
					{
						// Moving Up
						if (coord.y > square.coord.y)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.y > square.coord.y && current.coord.y < coord.y && current.piece.type != Piece.PieceType.none)
							{
								return;
							}

						}

						// Moving Down
						if (coord.y < square.coord.y)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.y < square.coord.y && current.coord.y > coord.y && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}
				}
				MovePiece();
			}
		}
		#endregion

		#region Bishop
		if (pType == Piece.PieceType.bishop)
		{
			if ((square.index - index) %9 == 0 || (square.index - index) %7 == 0)
			{
				foreach (Transform listSquare in Board.Instance.transform)
				{
					Square current = listSquare.GetComponent<Square>();
					
					// Moving Right Up or Left Down
					if ((current.index - index) % 9 == 0)
					{
						print(current.coord);
						// Moving up right
						if (index > square.index)
						{
							if (current.index > square.index && current.index < index && current.piece.type != Piece.PieceType.none)
							{
								print(current.piece.type +" at " + current.coord);
								return;
							}
						}

						// Moving down left
						if (index < square.index)
						{
							if (current.index < square.index && current.index > index && current.piece.type != Piece.PieceType.none)
							{
								print(current.piece.type + " at " + current.coord);
								return;
							}
						}
					}

					// Moving Down Right or Up Left
					if ((current.index - index) % 7 == 0)
					{
						print(current.coord);
						// Moving up right
						if (index > square.index)
						{
							if (current.index > square.index && current.index < index && current.piece.type != Piece.PieceType.none)
							{
								print(current.piece.type + " at " + current.coord);
								return;
							}
						}

						// Moving down left
						if (index < square.index)
						{
							if (current.index < square.index && current.index > index && current.piece.type != Piece.PieceType.none)
							{
								print(current.piece.type + " at " + current.coord);
								return;
							}
						}
					}

				}
				
			MovePiece();
			}
		}
		#endregion

		#region Queen
		if (pType == Piece.PieceType.queen)
		{
			if ((square.index - index) % 9 == 0 || (square.index - index) % 7 == 0 || square.coord.x == coord.x || square.coord.y == coord.y)
			{
				foreach (Transform listSquare in Board.Instance.transform)
				{
					Square current = listSquare.GetComponent<Square>();
					#region Horizontal/Vertical

					// Checking the file of the rook
					if (current.coord.y == coord.y)
					{
						// Moving to the right
						if (coord.x > square.coord.x)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.x > square.coord.x && current.coord.x < coord.x && current.piece.type != Piece.PieceType.none)
							{
								return;
							}

						}

						// Moving to the Left
						if (coord.x < square.coord.x)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.x < square.coord.x && current.coord.x > coord.x && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}

					// ------------------------------------------------------------------

					// Checking the file of the rook
					if (current.coord.x == coord.x)
					{
						// Moving Up
						if (coord.y > square.coord.y)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.y > square.coord.y && current.coord.y < coord.y && current.piece.type != Piece.PieceType.none)
							{
								return;
							}

						}

						// Moving Down
						if (coord.y < square.coord.y)
						{
							// Checking between final and initial square to see if there is a piece there
							if (current.coord.y < square.coord.y && current.coord.y > coord.y && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}
					#endregion

					#region Diagonal
					if ((current.index - index) % 9 == 0)
					{
						// Moving up-right
						if (index - square.index > 0)
						{
							if (current.index > square.index && current.index < index && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}

						// Moving up-right
						if (index - square.index < 0)
						{
							if (current.index < square.index && current.index > index && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}

					// -----------------------------------------------------------------

					if ((current.index - index) % 7 == 0)
					{
						// Moving up-right
						if (index - square.index > 0)
						{
							if (current.index > square.index && current.index < index && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}

						// Moving up-right
						if (index - square.index < 0)
						{
							if (current.index < square.index && current.index > index && current.piece.type != Piece.PieceType.none)
							{
								return;
							}
						}
					}
				}
				#endregion
				}
				MovePiece();
		}
		
		#endregion
	}

}



