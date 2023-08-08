using System;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public string serverAddress = "127.0.0.1";
    public int serverPort = 8888;

    private TcpClient client;
    private NetworkStream networkStream;

    private byte[] receiveBuffer = new byte[4096];
    private string receivedData = "";

    public GameManager gameManager; // Reference to the GameManager instance

    private UnityMainThreadDispatcher unityMainThreadDispatcher; // Reference to the UnityMainThreadDispatcher instance

    private Server server; // Store the Server reference

    private void Start()
    {
        unityMainThreadDispatcher = UnityMainThreadDispatcher.Instance(); // Initialize the UnityMainThreadDispatcher
        gameManager = FindObjectOfType<GameManager>(); // Find and set the GameManager reference programmatically

        server = FindObjectOfType<Server>();

        if (gameManager != null)
        {
            Debug.Log("GameManager reference assigned.");
        }
        else
        {
            Debug.LogWarning("GameManager reference not found.");
        }
    }

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.BeginConnect(serverAddress, serverPort, ConnectCallback, null);
        }
        catch (Exception e)
        {
            Debug.Log("Error connecting to server: " + e.Message);
        }
    }

    public void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            client.EndConnect(ar);
            if (client.Connected)
            {
                Debug.Log("Connected to server.");

                networkStream = client.GetStream();
                networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);


            }
            else
            {
                Debug.Log("Failed to connect to server.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error during connect callback: " + e.Message);
        }
    }

    public void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int bytesRead = networkStream.EndRead(ar);
            if (bytesRead <= 0)
            {
                Debug.Log("Disconnected from server.");
                client.Close();
                return;
            }

            byte[] data = new byte[bytesRead];
            Array.Copy(receiveBuffer, data, bytesRead);
            receivedData = System.Text.Encoding.ASCII.GetString(data);

            // Process the received data
            ProcessData(receivedData);

            // Continue reading from the stream
            networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Debug.Log("Error during receive callback: " + e.Message);
        }
    }

    public void ProcessData(string data)
    {
        string[] messages = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string message in messages)
        {
            Debug.Log("Received message: " + message);

            if (message.StartsWith("PLAYER_NUMBER:"))
            {
                int playerNumber = int.Parse(message.Substring(14));
                Debug.Log("Assigned player number: " + playerNumber);
                // Use the player number for any relevant logic in your client
            }
            else if (message.StartsWith("START_GAME:"))
            {
                int firstPlayer = int.Parse(message.Substring(11));
                Debug.Log("Starting game with first player: " + firstPlayer);

                // Update the GameManager on both clients
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    gameManager.StartGame(firstPlayer);
                    gameManager.UpdateCurrentPlayerText(); // Update the UI text
                });
            }
            else if (message.StartsWith("PLAYERTURN:"))
            {
                int currentPlayer = int.Parse(message.Substring(11));
                Debug.Log("Player's turn: " + currentPlayer);

                // Update UI to indicate the current player's turn
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    gameManager.UpdateCurrentPlayerText();
                });
            }
            else if (message.StartsWith("MOVE:"))
            {
                // Extract the move index from the message
                int moveIndex = int.Parse(message.Substring(5));
                // Call the GameManager's MakeMove function with the received move index
                //gameManager.MakeMove(moveIndex);
            }
            else if (message == "WIN")
            {
                // Call the GameManager's HandleWin function with the player index
                //gameManager.HandleWin(gameManager.currentPlayer);
            }
            else if (message == "DRAW")
            {
                // Call the GameManager's HandleDraw function
                //gameManager.HandleDraw();
            }
        }
    }

    public void SendData(string data)
    {
        try
        {
            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data + "\n");
            networkStream.Write(dataBuffer, 0, dataBuffer.Length);
        }
        catch (Exception e)
        {
            Debug.Log("Error sending data: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        if (client != null && client.Connected)
        {
            client.Close();
        }
    }
}