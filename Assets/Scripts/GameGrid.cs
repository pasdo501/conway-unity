using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private float[,] grid;
    private int vertical;
    private int horizontal;
    private int cols;
    private int rows;
    private Sprite sprite;

    public GameGrid(Sprite sprite)
    {
        this.sprite = sprite;

        vertical = (int) Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);

        cols = horizontal * 2;
        rows = vertical * 2;

        grid = new float[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                float val = Random.Range(0f, 1f);
                grid[i, j] = val;
                populateCell(i, j, val);
            }
        }
    }

    private void populateCell(int y, int x, float val)
    {
        GameObject g = new GameObject($"X: {x}; Y: {y}");
        g.transform.position = new Vector2(x - (horizontal -.5f), y - (vertical - .5f));
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = val <= .2f ? Color.white : Color.black;
    }
}