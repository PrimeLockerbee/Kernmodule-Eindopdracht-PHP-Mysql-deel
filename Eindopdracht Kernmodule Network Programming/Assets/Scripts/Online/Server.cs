using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    private const int Port = 8888;
    private const int MaxClients = 2;

    private TcpListener serverSocket;
    private TcpClient[] clients;
    private NetworkStream[] clientStreams;
    private byte[] receiveBuffer;

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
        int clientIndex = Array.IndexOf(clients, null);
        clients[clientIndex] = client;
        clientStreams[clientIndex] = client.GetStream();
        clientStreams[clientIndex].BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveMessage, clientIndex);
        serverSocket.BeginAcceptTcpClient(ClientConnected, null);
        Debug.Log("Client connected. Index: " + clientIndex);
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
        byte[] messageData = Encoding.ASCII.GetBytes(message);
        clientStreams[clientIndex].Write(messageData, 0, messageData.Length);
    }
}