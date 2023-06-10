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

    private void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.BeginConnect(serverAddress, serverPort, ConnectCallback, null);
            gameManager.UpdateCurrentPlayerText();
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

            // Start listening for more data
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
            if (message.StartsWith("MOVE:"))
            {
                gameManager.UpdateCurrentPlayerText();
                // Extract the move index from the message
                int moveIndex = int.Parse(message.Substring(5));
                // Call the GameManager's MakeMove function with the received move index
                gameManager.MakeMove(moveIndex);
            }
            else if (message == "WIN")
            {
                // Call the GameManager's HandleWin function with the player index
                gameManager.HandleWin(gameManager.currentPlayer);
            }
            else if (message == "DRAW")
            {
                // Call the GameManager's HandleDraw function
                gameManager.HandleDraw();
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