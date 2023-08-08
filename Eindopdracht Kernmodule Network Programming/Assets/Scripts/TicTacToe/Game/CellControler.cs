using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellControler : MonoBehaviour
{
    public int index; // Add the index variable

    private Button button;
    private TicTacToeGameManager gameManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<TicTacToeGameManager>();
    }

    public void HandleCellClick()
    {
        if (gameManager != null)
        {
            gameManager.MakeMove(index);
        }
    }
}
