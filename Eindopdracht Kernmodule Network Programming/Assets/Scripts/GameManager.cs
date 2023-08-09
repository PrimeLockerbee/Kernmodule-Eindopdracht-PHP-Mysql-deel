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

    private Server server;

    public Button[] cellButtons;

    private int playerNumber; // Store the player number for this client

    public void SetServer(Server server)
    {
        this.server = server;
    }

    public void StartGame(int firstPlayer)
    {
        //Debug.Log("StartGame called with firstPlayer: " + firstPlayer);
        //Debug.Log("StartGame called. Current player: " + currentPlayer);

        this.currentPlayer = firstPlayer;
        isGameOver = false;

        UpdateCurrentPlayerText();
    }


    public void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("SwitchPlayer called. Current player: " + currentPlayer);
        UpdateCurrentPlayerText(); // Add this line to update the UI text
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
}