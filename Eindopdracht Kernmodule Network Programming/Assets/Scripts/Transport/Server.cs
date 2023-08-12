using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class Server : MonoBehaviour
{
    private const int Port = 8888;
    private const int MaxClients = 2;

    private TcpListener serverSocket;
    private TcpClient[] clients;
    private NetworkStream[] clientStreams;
    private byte[] receiveBuffer;
    private bool isGameActive;

    public GameManager startGame;

    private int connectedClients;
    private int nextPlayerNumber = 1;

    HashSet<int> assignedPlayerNumbers = new HashSet<int>(); // Keep track of assigned player numbers

    public void StartGameServer()
    {
        InitializeServer();

        startGame = FindObjectOfType<GameManager>();
        if (startGame != null)
        {
            startGame.SetServer(this);
        }
        else
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    public void InitializeServer()
    {
        serverSocket = new TcpListener(IPAddress.Any, Port);
        serverSocket.Start();
        clients = new TcpClient[MaxClients];
        clientStreams = new NetworkStream[MaxClients];
        receiveBuffer = new byte[1024];
        serverSocket.BeginAcceptTcpClient(ClientConnected, null);
        Debug.Log("Server started. Waiting for clients...");
    }

    private void ClientConnected(IAsyncResult ar)
    {
        TcpClient client = serverSocket.EndAcceptTcpClient(ar);
        if (!isGameActive)
        {
            int clientIndex = Array.IndexOf(clients, null);
            if (clientIndex != -1)
            {
                clients[clientIndex] = client;
                clientStreams[clientIndex] = client.GetStream();
                clientStreams[clientIndex].BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveMessage, clientIndex);
                Debug.Log("Client connected. Index: " + clientIndex);

                connectedClients++;

                // Assign player numbers based on connection order
                int playerNumber = nextPlayerNumber;
                nextPlayerNumber++;

                assignedPlayerNumbers.Add(playerNumber); // Add the player number to the set of assigned numbers

                Debug.Log("Assigned player number: " + playerNumber);
                BroadcastMessageToClients("PLAYER_NUMBER:" + playerNumber);
            }
            else
            {
                Debug.Log("Maximum number of clients reached. Connection rejected.");
                client.Close();
            }
        }
        else
        {
            Debug.Log("Game is already active. Connection rejected.");
            client.Close();
        }

        serverSocket.BeginAcceptTcpClient(ClientConnected, null);
    }

    private void ReceiveMessage(IAsyncResult ar)
    {
        int clientIndex = (int)ar.AsyncState;
        NetworkStream clientStream = clientStreams[clientIndex];
        int bytesRead = clientStream.EndRead(ar);
        if (bytesRead <= 0)
        {
            // Client disconnected
            Debug.Log("Client disconnected. Index: " + clientIndex);
            clientStream.Close();
            clients[clientIndex].Close();
            clients[clientIndex] = null;
            clientStreams[clientIndex] = null;

            connectedClients--;

            if (isGameActive)
            {
                // Handle game logic when a player disconnects during an active game
            }

            return;
        }

        byte[] messageData = new byte[bytesRead];
        Array.Copy(receiveBuffer, messageData, bytesRead);
        string message = Encoding.ASCII.GetString(messageData);
        Debug.Log("Received message from client " + clientIndex + ": " + message);

        // Process the received message and send updates back to clients

        clientStreams[clientIndex].BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveMessage, clientIndex);
    }

    private void SendMessageToClient(int clientIndex, string message)
    {
        if (clients[clientIndex] != null && clientStreams[clientIndex] != null)
        {
            byte[] messageData = Encoding.ASCII.GetBytes(message);
            clientStreams[clientIndex].Write(messageData, 0, messageData.Length);
            Debug.Log("Sent message to client " + clientIndex + ": " + message);
        }
    }

    public void BroadcastMessageToClients(string message)
    {
        string delimitedMessage = message + "###"; // Add delimiter
        for (int i = 0; i < connectedClients; i++)
        {
            if (clients[i] != null)
            {
                SendMessageToClient(i, delimitedMessage);
            }
        }
    }

    public void BroadcastSwitchPlayer()
    {
        BroadcastMessageToClients("SWITCH_PLAYER");
    }

    public void BroadcastWin()
    {
        BroadcastMessageToClients("WIN");
    }

    public void BroadcastDraw()
    {
        BroadcastMessageToClients("DRAW");
    }

    public void BroadcastUpdateVisual()
    {
        BroadcastMessageToClients("UPDATE_VISUAL");
    }

    private void Update()
    {
        if (!isGameActive && connectedClients == MaxClients)
        {
            // Determine the first player (randomly in this example)
            int firstPlayer = UnityEngine.Random.Range(1, MaxClients + 1);

            Debug.Log("Max clients connected, game starting");

            // Broadcast the start of the game and the first player's number
            BroadcastMessageToClients("START_GAME:" + firstPlayer);

            // Set the game as active
            isGameActive = true;
        }
    }

}