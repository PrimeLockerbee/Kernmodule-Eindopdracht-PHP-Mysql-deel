using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI gameResultText;
    public TextMeshProUGUI playerNumberText;

    public Color player1Color = Color.red;
    public Color player2Color = Color.blue;

    public int currentPlayer;
    private int[,] gameBoard;
    private bool isGameOver;

    public Server server;

    public Button[] cellButtons;

    public int playerNumber; // Store the player number for this client

    public GameState gameState = new GameState();

    public void SetServer(Server server)
    {
        this.server = server;
    }

    public void StartGame(int firstPlayer)
    {
        this.currentPlayer = firstPlayer;
        gameState.SetCurrentPlayer(firstPlayer); // Update the current player in GameState
        GameBoardThingy();
        isGameOver = false;

        UpdateCurrentPlayerText();
    }

    public void MakeMove(int cellIndex)
    {
        if (gameState.isGameOver || !gameState.IsCellEmpty(cellIndex) || gameState.currentPlayer != playerNumber)
            return;

        gameState.MakeMove(cellIndex, playerNumber); // Use the MakeMove from GameState

        // Update the visual representation of the game board with the currentPlayer's marker
        UpdateCellVisual(cellIndex, playerNumber);

        // Send move information to all clients
        string moveData = "MOVE:" + cellIndex;
        server.BroadcastMessageToClients(moveData);
        Debug.Log("Move data sent: " + moveData);

        // Check for a win condition or a draw
        if (gameState.CheckWinCondition(playerNumber))
        {
            server.BroadcastWin();
        }
        else if (gameState.CheckDraw())
        {
            server.BroadcastDraw();
        }
        else
        {
            // Switch to the next player's turn
            gameState.SwitchPlayer();
            server.BroadcastSwitchPlayer();
        }
    }

    public void UpdateCellVisual(int index, int playerNumber)
    {
        string cellName = "Cell_" + (index + 1);
        GameObject cellObject = GameObject.Find(cellName);

        if (cellObject != null)
        {
            TextMeshProUGUI buttonText = cellObject.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                // Set the visual representation based on the player's marker
                buttonText.text = (playerNumber == 1) ? "X" : "O";

                // Get the existing color
                Color existingColor = buttonText.color;

                // Set the new color with the existing alpha value set to maximum (255)
                buttonText.color = (playerNumber == 1)
                    ? new Color(player1Color.r, player1Color.g, player1Color.b, 255f / 255f)
                    : new Color(player2Color.r, player2Color.g, player2Color.b, 255f / 255f);
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on cell: " + cellName);
            }
        }
        else
        {
            Debug.LogError("Cell object not found: " + cellName);
        }
    }

    public void HandleWin(int player)
    {
        isGameOver = true;
        gameResultText.text = "Player " + player + " wins!";

        // Set text color based on the winning player with alpha value set to maximum (255)
        gameResultText.color = (player == 1)
            ? new Color(player1Color.r, player1Color.g, player1Color.b, 255f / 255f)
            : new Color(player2Color.r, player2Color.g, player2Color.b, 255f / 255f);
    }

    public void HandleDraw()
    {
        isGameOver = true;
        gameResultText.text = "It's a draw!";

        gameResultText.color = new Color(0, 0, 0, 255f / 255f);
    }

    public void UpdateCurrentPlayerText()
    {
        currentPlayerText.text = "Current Player: " + currentPlayer;

        // Set text color based on current player with alpha value set to maximum (255)
        currentPlayerText.color = (currentPlayer == 1)
            ? new Color(player1Color.r, player1Color.g, player1Color.b, 255f / 255f)
            : new Color(player2Color.r, player2Color.g, player2Color.b, 255f / 255f);
    }

    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
        playerNumberText.text = "Player Number: " + playerNumber;
    }

    private void GameBoardThingy()
    {
        gameBoard = new int[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gameBoard[i, j] = 0; // Initialize with empty cells
            }
        }
    }
}