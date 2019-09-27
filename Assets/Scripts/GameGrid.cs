using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class responsible for the Grid displayed in the simulation.
/// </summary>
public class GameGrid
{
    /// <summary> 
    /// The Grid represented as an array
    /// </summary>
    private Cell[,] grid;
    /// <summary>
    /// Vertical window size (based on the camera)
    /// </summary>
    private int vertical;
    /// <summary>
    /// Horizontal window size (based on the camera)
    /// </summary>
    private int horizontal;
    /// <summary>
    /// Number of columns in the grid
    /// </summary>
    private int cols;
    /// <summary>
    /// Number of rows in the grid
    /// </summary>
    private int rows;
    /// <summary>
    /// Sprite component property
    /// </summary>
    private Sprite sprite;

    /// <summary>The class constructor. Populates the grid with cells that are
    /// randomly determined to be either dead or alive. The chances of a cell
    /// being alive are 20%.</summary>
    /// <param name="sprite">
    /// The sprite to be used by individual cells in the grid.
    /// </param>
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

    /// <summary>
    /// Method to update the grid after each 'generation'.
    /// 
    /// Dead cells with 3 live neighbours will become alive in the next
    /// generation, while live cells with fewer than 1 or more than 4
    /// neighbours will die in the next generation.
    /// </summary>
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

    /// <summary>
    /// Method to count the number of living neighbours of a cell in a given
    /// <paramref name="row"/> and <paramref name="column"/>.
    /// </summary>
    /// <param name="row">The row of the cell</param>
    /// <param name="column">The column of the cell</param>
    /// <returns>The number of live neighbours of the cell</returns>
    private int LiveNeighbourCount(int row, int column)
    {
        int neighbourCount = 0;

        int[,] neighbours = {
            // Above
            {-1, -1}, {-1, 0}, {-1, 1},
            // Sides
            {0, -1}, {0, 1},
            // Below
            {1, -1}, {1, 0}, {1, 1}
        };

        for (int i = 0; i < neighbours.GetLength(0); i++) {
            int y = WrapIndex(row, neighbours[i, 0], this.rows);
            int x = WrapIndex(column, neighbours[i, 1], this.cols);

            if (grid[y, x].Alive)
                neighbourCount++;
        }

        return neighbourCount;
    }

    /// <summary>
    /// Method to calculte a new index from a given <paramref name="index"/>
    /// and <paramref name="offset"/>, while wrapping around a given array 
    /// <paramref name="size"/>.
    /// </summary>
    /// <param name="index">The initial index</param>
    /// <param name="offset">The offset from the index</param>
    /// <param name="size">The size of the array</param>
    /// <returns>The new index</returns>
    private int WrapIndex(int index, int offset, int size)
    {
        int res = index + offset;

        if (res < 0)
            return size - 1;

        if (res == size)
            return 0;

        return res;
    }
}