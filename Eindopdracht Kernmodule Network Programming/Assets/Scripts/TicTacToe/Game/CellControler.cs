using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellControler : MonoBehaviour
{
    public int index; // Add the index variable

    private Button button;
    public GameManager gameManager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void HandleCellClick()
    {
        if (gameManager != null)
        {
            // Call the GameState's MakeMove function with the cell index and player number
            gameManager.gameState.MakeMove(index, gameManager.playerNumber);
        }
    }
}