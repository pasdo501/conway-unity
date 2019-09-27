using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private Cell[,] grid;
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

        grid = new Cell[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                float val = Random.Range(0f, 1f);
                bool alive = val <= .2f;
                Vector2 position = new Vector2(j - (horizontal -.5f), i - (vertical - .5f));
                Cell c = new Cell(sprite, alive, position);
                grid[i, j] = c;
            }
        }
    }

    public void Update()
    {
        Stack<Cell> toUpdate = new Stack<Cell>();
        for (int row = 0; row < this.rows; row++) {
            for (int col = 0; col < this.cols; col++) {
                Cell current = grid[row, col];
                int neighbourCount = LiveNeighbourCount(row, col);
                if (!current.Alive) {
                    if (neighbourCount == 3) {
                        toUpdate.Push(current);
                    }
                } else {
                    if (neighbourCount <= 1 || neighbourCount >= 4) {
                        toUpdate.Push(current);
                    }
                }
            }
        }

        while (toUpdate.Count > 0) {
            Cell c = toUpdate.Pop();
            c.ToggleState();
        }
    }

    private int LiveNeighbourCount(int row, int column)
    {
        int neighbourCount = 0;

        int[,] neighbours = {
            {-1, -1}, {-1, 0}, {-1, 1},
            {0, -1}, {0, 1},
            {1, -1}, {1, 0}, {1, 1}
        };

        for (int i = 0; i < neighbours.GetLength(0); i++) {
            int y = neighbours[i, 0] + row;
            if (y < 0) {
                y = this.rows - 1;
            } else if (y == this.rows) {
                y = 0;
            }

            int x = neighbours[i, 1] + column;
            if (x < 0) {
                x = this.cols - 1;
            } else if (x == this.cols) {
                x = 0;
            }

            if (grid[y, x].Alive)
                neighbourCount++;
        }

        return neighbourCount;
    }
}