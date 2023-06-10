using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI gameResultText;
    public Button[] cellButtons;

    public Color player1Color = Color.red;
    public Color player2Color = Color.blue;

    public int currentPlayer;
    private int[,] gameBoard;
    private bool isGameOver;

    private Server server;

    public void SetServer(Server server)
    {
        this.server = server;
    }

    public void StartGame(int currentPlayer)
    {
        this.currentPlayer = currentPlayer;
        gameBoard = new int[3, 3];
        isGameOver = false;


        // Update the text components of each cell with their respective indices
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                int index = row * 3 + column;
                GameObject cellParentObject = GameObject.Find("Cell_" + (row + 1) + (column + 1));

                if (cellParentObject != null)
                {
                    Button cellButton = cellParentObject.GetComponentInChildren<Button>();
                    cellButton.onClick.AddListener(() => MakeMove(index));

                    CellControler cellController = cellParentObject.GetComponentInChildren<CellControler>();

                    if (cellController != null)
                    {
                        TextMeshProUGUI buttonText = cellController.GetComponentInChildren<TextMeshProUGUI>();

                        if (buttonText != null)
                        {
                            buttonText.text = "";
                            buttonText.color = (currentPlayer == 1) ? player1Color : player2Color; // Set text color based on current player
                        }
                        else
                        {
                            Debug.LogError("TextMeshProUGUI component not found on cell: " + cellParentObject.name);
                        }
                    }
                    else
                    {
                        Debug.LogError("CellControler component not found on cell: " + cellParentObject.name);
                    }
                }
                else
                {
                    Debug.LogError("Cell object not found: Cell_" + (row + 1) + (column + 1));
                }
            }
        }

        UpdateCurrentPlayerText();
    }

    public void MakeMove(int cellIndex)
    {
        if (isGameOver)
            return;

        // Check if the cell is already occupied
        if (!IsCellEmpty(cellIndex))
            return;

        // Convert the cell index to row and column
        int row = cellIndex / 3;
        int column = cellIndex % 3;

        // Set the cell with the current player's marker (1 for Player 1, 2 for Player 2)
        gameBoard[row, column] = currentPlayer;

        // Update the visual representation of the game board
        UpdateCellVisual(cellIndex);

        // Check for a win condition or a draw
        if (CheckWinCondition(currentPlayer))
        {
            HandleWin(currentPlayer);
        }
        else if (CheckDraw())
        {
            HandleDraw();
        }
        else
        {
            // Switch to the next player's turn
            SwitchPlayer();
            UpdateCurrentPlayerText();
        }

        // Send move information to all clients
        string moveData = "MOVE:" + cellIndex;
        server.BroadcastMessageToClients(moveData);
    }

    public void UpdateCellVisual(int index)
    {
        string cellName = "Cell_" + (index + 1);
        GameObject cellObject = GameObject.Find(cellName);

        if (cellObject != null)
        {
            TextMeshProUGUI buttonText = cellObject.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                // Set the visual representation based on the current player
                buttonText.text = (currentPlayer == 1) ? "X" : "O";

                // Get the existing color
                Color existingColor = buttonText.color;

                // Set the new color with the existing alpha value set to maximum (255)
                buttonText.color = (currentPlayer == 1)
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

    public void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }

    public bool CheckWinCondition(int player)
    {
        // Check for horizontal win conditions
        if (gameBoard[0, 0] == player && gameBoard[0, 1] == player && gameBoard[0, 2] == player)
            return true;
        if (gameBoard[1, 0] == player && gameBoard[1, 1] == player && gameBoard[1, 2] == player)
            return true;
        if (gameBoard[2, 0] == player && gameBoard[2, 1] == player && gameBoard[2, 2] == player)
            return true;

        // Check for vertical win conditions
        if (gameBoard[0, 0] == player && gameBoard[1, 0] == player && gameBoard[2, 0] == player)
            return true;
        if (gameBoard[0, 1] == player && gameBoard[1, 1] == player && gameBoard[2, 1] == player)
            return true;
        if (gameBoard[0, 2] == player && gameBoard[1, 2] == player && gameBoard[2, 2] == player)
            return true;

        // Check for diagonal win conditions
        if (gameBoard[0, 0] == player && gameBoard[1, 1] == player && gameBoard[2, 2] == player)
            return true;
        if (gameBoard[0, 2] == player && gameBoard[1, 1] == player && gameBoard[2, 0] == player)
            return true;

        return false; // Return true if a win condition is met
    }

    public bool CheckDraw()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (gameBoard[row, column] == 0)
                    return false; // If any cell is empty, it's not a draw
            }
        }

        return true; // Return true if all cells are occupied (draw)
    }

    public void HandleWin(int player)
    {
        isGameOver = true;
        gameResultText.text = "Player " + player + " wins!";

        // Set text color based on the winning player with alpha value set to maximum (255)
        gameResultText.color = (player == 1)
            ? new Color(player1Color.r, player1Color.g, player1Color.b, 255f / 255f)
            : new Color(player2Color.r, player2Color.g, player2Color.b, 255f / 255f);

        // Broadcast win message to clients
        server.BroadcastMessageToClients("WIN:" + player);
    }

    public void HandleDraw()
    {
        isGameOver = true;
        gameResultText.text = "It's a draw!";

        // Broadcast draw message to clients
        server.BroadcastMessageToClients("DRAW");
    }

    public void UpdateCurrentPlayerText()
    {
        currentPlayerText.text = "Current Player: " + currentPlayer;

        // Set text color based on current player with alpha value set to maximum (255)
        currentPlayerText.color = (currentPlayer == 1)
            ? new Color(player1Color.r, player1Color.g, player1Color.b, 255f / 255f)
            : new Color(player2Color.r, player2Color.g, player2Color.b, 255f / 255f);
    }

    public bool IsCellEmpty(int cellIndex)
    {
        int row = cellIndex / 3;
        int column = cellIndex % 3;
        return gameBoard[row, column] == 0;
    }
}