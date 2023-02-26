using UnityEngine;
using UnityEngine.UI;

public class Chessboard : MonoBehaviour
{
    public int boardSize = 8; // Tamanho do tabuleiro
    public int tileSize = 64; // Tamanho de cada casa
    public Color lightSquareColor = new Color(0.8f, 0.8f, 0.8f); // Cor das casas claras
    public Color darkSquareColor = new Color(0.3f, 0.3f, 0.3f); // Cor das casas escuras

    void Start()
    {
        // Obtém o componente Image do objeto
        Image image = GetComponent<Image>();

        // Cria uma textura para desenhar o tabuleiro
        Texture2D texture = new Texture2D(boardSize * tileSize, boardSize * tileSize);

        // Loop para desenhar cada casa do tabuleiro
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                // Calcula a cor da casa atual
                Color color = ((x + y) % 2 == 0) ? lightSquareColor : darkSquareColor;

                // Pinta a casa na textura
                for (int i = 0; i < tileSize; i++)
                {
                    for (int j = 0; j < tileSize; j++)
                    {
                        texture.SetPixel(x * tileSize + i, y * tileSize + j, color);
                    }
                }
            }
        }

        // Aplica as mudanças na textura
        texture.Apply();

        // Define a textura como a imagem do objeto
        image.sprite = Sprite.Create(texture, new Rect(0, 0, boardSize * tileSize, boardSize * tileSize), new Vector2(0.5f, 0.5f));
    }
}
