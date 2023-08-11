using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameState
{
    public int[,] gameBoard;
    public int currentPlayer;
    public bool isGameOver;

    public Server server;

    public GameState()
    {
        gameBoard = new int[3, 3];
        currentPlayer = 1;
    }

    //Add methods to update game state and handle synchronization
    public void MakeMove(int cellIndex, int playerNumber)
    {
        int row = cellIndex / 3;
        int column = cellIndex % 3;
        gameBoard[row, column] = playerNumber;

        Debug.Log("Move works");
    }

    public void SetCurrentPlayer(int playerNumber)
    {
        currentPlayer = playerNumber;
    }

    public bool CheckWinCondition(int player)
    {
        //Check for horizontal win conditions
        if (gameBoard[0, 0] == player && gameBoard[0, 1] == player && gameBoard[0, 2] == player)
            return true;
        if (gameBoard[1, 0] == player && gameBoard[1, 1] == player && gameBoard[1, 2] == player)
            return true;
        if (gameBoard[2, 0] == player && gameBoard[2, 1] == player && gameBoard[2, 2] == player)
            return true;

        //Check for vertical win conditions
        if (gameBoard[0, 0] == player && gameBoard[1, 0] == player && gameBoard[2, 0] == player)
            return true;
        if (gameBoard[0, 1] == player && gameBoard[1, 1] == player && gameBoard[2, 1] == player)
            return true;
        if (gameBoard[0, 2] == player && gameBoard[1, 2] == player && gameBoard[2, 2] == player)
            return true;

        //Check for diagonal win conditions
        if (gameBoard[0, 0] == player && gameBoard[1, 1] == player && gameBoard[2, 2] == player)
            return true;
        if (gameBoard[0, 2] == player && gameBoard[1, 1] == player && gameBoard[2, 0] == player)
            return true;

        return false; //Return true if a win condition is met
    }

    public bool CheckDraw()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (gameBoard[row, column] == 0)
                    return false; //If any cell is empty, it's not a draw
            }
        }

        return true; //Return true if all cells are occupied (draw)
    }

    public void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("SwitchPlayer works");
    }

    public bool IsCellEmpty(int cellIndex)
    {
        int row = cellIndex / 3;
        int column = cellIndex % 3;
        return gameBoard[row, column] == 0;
    }
}
