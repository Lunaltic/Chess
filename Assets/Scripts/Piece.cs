using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType type = PieceType.none;
    public PieceColor pieceColor;

    SpriteRenderer spriteRenderer;

	private void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
        if(pieceColor == PieceColor.white)
		{
            switch (type)
            {
                case PieceType.none:
                    spriteRenderer.sprite = null;
                    break;
                case PieceType.pawn:
                    spriteRenderer.sprite = Board.Instance.whitePawn;
                    break;
                case PieceType.knight:
                    spriteRenderer.sprite = Board.Instance.whiteKnight;
                    break;
                case PieceType.bishop:
                    spriteRenderer.sprite = Board.Instance.whiteBishop; 
                    break;
                case PieceType.rook:
                    spriteRenderer.sprite = Board.Instance.whiteRook; 
                    break;
                case PieceType.queen:
                    spriteRenderer.sprite = Board.Instance.whiteQueen; 
                    break;
                case PieceType.king:
                    spriteRenderer.sprite = Board.Instance.whiteKing; 
                    break;
                default:
                    break;
            }
		}
		else
		{
            switch (type)
            {
                case PieceType.none:
                    spriteRenderer.sprite = null;
                    break;
                case PieceType.pawn:
                    spriteRenderer.sprite = Board.Instance.blackPawn; 
                    break;
                case PieceType.knight:
                    spriteRenderer.sprite = Board.Instance.blackKnight; 
                    break;
                case PieceType.bishop:
                    spriteRenderer.sprite = Board.Instance.blackBishop; 
                    break;
                case PieceType.rook:
                    spriteRenderer.sprite = Board.Instance.blackRook; 
                    break;
                case PieceType.queen:
                    spriteRenderer.sprite = Board.Instance.blackQueen; 
                    break;
                case PieceType.king:
                    spriteRenderer.sprite = Board.Instance.blackKing; 
                    break;
                default:
                    spriteRenderer.sprite = null;
                    break;
            }
        }
		
	}

	public enum PieceType
	{
        none, pawn, knight, bishop, rook, queen, king
	}

    public enum PieceColor
    {
        white, black
    }
}
