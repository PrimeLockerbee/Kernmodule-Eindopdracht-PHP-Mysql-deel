using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI gameResultText;

    public Color player1Color = Color.red;
    public Color player2Color = Color.blue;

    public int currentPlayer;

    private bool isGameOver;

    private Server server;

    public void SetServer(Server server)
    {
        this.server = server;
    }

    public void StartGame(int firstPlayer)
    {
        Debug.Log("StartGame called with firstPlayer: " + firstPlayer);

        //is.gameObject = new GameManager();

        currentPlayer = firstPlayer;

        isGameOver = false;

        Debug.Log("StartGame called. Current player: " + currentPlayer);

        // Update the currentPlayerText on all clients
        server.BroadcastMessageToClients("PLAYERTURN:" + currentPlayer);

        UpdateCurrentPlayerText();
    }

    public void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("SwitchPlayer called. Current player: " + currentPlayer);
        UpdateCurrentPlayerText(); // Add this line to update the UI text
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
}