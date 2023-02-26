using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameStatus status;
    public static GameManager Instance;

    public Vector2 selectedSquare;
    public Vector2 targetSquare;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum GameStatus
	{
        drawingBoard, readyToPlay, playing, gameOver
	}
}
