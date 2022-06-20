using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public BoardManager boardManager;
    public GameObject board;
    private int level = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        board = new GameObject("Board");
        board.AddComponent<NavMeshSurface>();

        boardManager.board = board;
        boardManager.SetupScene(level);
    }
}
