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


    private void Start()
    {
        startGame = FindObjectOfType<GameManager>();
        if (startGame != null)
        {
            startGame.SetServer(this);
        }
        else
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        InitializeServer();
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

                // Check if the desired number of players has been reached
                if (connectedClients == MaxClients)
                {
                    isGameActive = true;
                    Debug.Log("Game is now active!");

                    // Assign player numbers to clients
                    for (int i = 0; i < MaxClients; i++)
                    {
                        SendMessageToClient(i, "PLAYER_NUMBER:" + (i + 1));
                        BroadcastMessageToClients("THIS WORKS");
                    }

                    // Start the game here or notify the GameManager to start the game
                    startGame.StartGame(clientIndex + 1);
                }
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
        for (int i = 0; i < MaxClients; i++)
        {
            if (clients[i] != null)
            {
                SendMessageToClient(i, message);
            }
        }
    }
}