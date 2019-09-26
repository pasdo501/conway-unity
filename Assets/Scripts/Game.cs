using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int[,] Grid
    {
        get { return grid; }
        set { grid = value; }
    }
    private int[,] grid;
    private int vertical;
    private int horizontal;
    private int cols;
    private int rows;
    // Start is called before the first frame update
    void Start()
    {
        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);

        cols = horizontal * 2;
        rows = vertical * 2;

        Grid = new int[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                Grid[i, j] = Random.Range(0, 2);
            }
        }
        
        Debug.Log("Done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
