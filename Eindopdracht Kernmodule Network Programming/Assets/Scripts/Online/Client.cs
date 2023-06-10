using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private const string ServerAddress = "127.0.0.1";
    private const int ServerPort = 8888;

    private TcpClient clientSocket;
    private NetworkStream networkStream;
    private byte[] receiveBuffer;


    public void ConnectToServer()
    {
        clientSocket = new TcpClient();
        clientSocket.BeginConnect(ServerAddress, ServerPort, ConnectionCallback, null);
        receiveBuffer = new byte[1024];
    }

    private void ConnectionCallback(IAsyncResult ar)
    {
        clientSocket.EndConnect(ar);
        networkStream = clientSocket.GetStream();
        networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveMessage, null);
        Debug.Log("Connected to server");
    }

    private void ReceiveMessage(IAsyncResult ar)
    {
        int bytesRead = networkStream.EndRead(ar);
        if (bytesRead <= 0)
        {
            // Disconnected from server
            Debug.Log("Disconnected from server");
            networkStream.Close();
            clientSocket.Close();
            return;
        }

        byte[] messageData = new byte[bytesRead];
        Array.Copy(receiveBuffer, messageData, bytesRead);
        string message = Encoding.ASCII.GetString(messageData);
        Debug.Log("Received message from server: " + message);

        // Process the received message and update the game state

        networkStream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveMessage, null);
    }

    private void SendMessageToServer(string message)
    {
        byte[] messageData = Encoding.ASCII.GetBytes(message);
        networkStream.Write(messageData, 0, messageData.Length);
    }
}