using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellControler : MonoBehaviour
{
    public int index; // Add the index variable

    private Button button;
    private GameManager gameManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void HandleCellClick()
    {
        if (gameManager != null)
        {
            gameManager.MakeMove(index);
        }
    }
}
