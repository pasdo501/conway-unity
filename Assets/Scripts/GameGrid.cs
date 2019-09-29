using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class responsible for the Grid displayed in the simulation.
/// </summary>
public class GameGrid
{
    /// <summary> 
    /// Grid storing references to the Cells in the game. Just in charge
    /// of toggling colour on and off
    /// </summary>
    private Cell[,] grid;
    /// <summary>
    /// Vertical window size (based on the camera)
    /// </summary>
    private int cols;
    /// <summary>
    /// Number of rows in the grid
    /// </summary>
    private int rows;
    /// <summary>
    /// Threshold of neighbours required for a dead cell to become live
    /// </summary>
    private const int BIRTH_THRESH = 3;
    /// <summary>
    /// Threshold of neighbour count at or below which a live cell will die
    /// </summary>
    private const int ISOLATION_TRESH = 1;
    /// <summary>
    /// Threshold of neighbour count at or above which a live cell will die
    /// </summary>
    private const int OVERCROWDING_TRESH = 4;
    /// <summary>
    /// Current generation of the simulation
    /// </summary>
    private int generation = 0;
    /// <summary>
    /// The current population of live cells
    /// </summary>
    private int population = 0;
    /// <summary>
    /// Reference to the UI status text element
    /// </summary>
    private Text displayText;
    /// <summary>
    /// Grid keeping track of game data (cell state, neighbour count)
    /// </summary>
    /// <remark>
    /// Each cell is represented by a byte. The top 3 bits are not used,
    /// bits 5 - 2 store the live neighbour count of a cell, while the lowest
    /// bit determines whether the cell is alive (high) or dead (low).
    /// i.e. [XXXN NNNS] where X is an unused bit, N is a bit used to store
    /// neighbour count and S is used to track the cell's state.
    /// </remark>
    private byte[,] dataGrid;
    /// <summary>
    /// Offset to the 8 neighbours of an index in the grid.
    /// </summary>
    /// <value>
    /// In order: top left, top, top right, left, right, bottom left,
    /// bottom, bottom right.
    /// </value>
    private int[,] neighbours = {
        // Above
        {-1, -1}, {-1, 0}, {-1, 1},
        // Sides
        {0, -1}, {0, 1},
        // Below
        {1, -1}, {1, 0}, {1, 1}
    };


    /// <summary>
    /// The class constructor. Populates the grid with cells that are
    /// randomly determined to be either dead or alive. The chances of a cell
    /// being alive are 8%.
    /// </summary>
    /// <param name="sprite">
    /// The sprite to be used by individual cells in the grid.
    /// </param>
    public GameGrid(Sprite sprite)
    {
        displayText = GameObject.Find("StatusText")?.GetComponent<Text>();
        if (displayText == null) {
            Debug.Log("StatusText element or text component not found," +
                " is this intentional?");
        }

        int vertical = (int) Camera.main.orthographicSize;
        int horizontal = vertical * (Screen.width / Screen.height);

        cols = horizontal * 2;
        rows = vertical * 2;


        grid = new Cell[rows, cols];
        dataGrid = new byte[rows, cols];

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                float val = Random.Range(0f, 1f);
                bool alive = val <= .08f;
                Vector2 position = new Vector2(j - (horizontal -.5f), i - (vertical - .5f));
                Cell c = new Cell(sprite, alive, position);
                grid[i, j] = c;
                if (alive) {
                    population++;
                    dataGrid[i, j] = 0x01;
                } else {
                    dataGrid[i, j] = 0x00;
                }
            }
        }
        InitialiseNeighbourCount();
        UpdateText();
    }

    /// <summary>
    /// Initialises the neighbour count of each cell in the grid. Neighbour
    /// count is stored in bits  - 1 of a byte
    /// </summary>
    private void InitialiseNeighbourCount()
    {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                for (int k = 0; k < neighbours.GetLength(0); k++) {
                    int y = WrapIndex(i, neighbours[k, 0], this.rows);
                    int x = WrapIndex(j, neighbours[k, 1], this.cols);

                    if ((dataGrid[y, x] & 0x01) == 0x01) {
                        dataGrid[i, j] += 2;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Toggles a cell in a given <paramref name="row"/> and 
    /// <paramref name="col"/> from alive to dead. The cell's neigbours'
    /// live neighbour counts are also updated to reflect this change
    /// </summary>
    /// <param name="row">The row of the cell being toggled</param>
    /// <param name="col">The column of the cell being toggled</param>
    private void ToggleCell(int row, int col)
    {
        int delta;
        if ((dataGrid[row, col] & 0x01) == 0x01) {
            delta = -2;
            population--;
        } else {
            delta = 2;
            population++;
        }

        // Toggle the state
        dataGrid[row, col] ^= 0x01;

        // Update neighbours' neighbour counts
        for (int i = 0; i < neighbours.GetLength(0); i++) {
            int y = WrapIndex(row, neighbours[i, 0], this.rows);
            int x = WrapIndex(col, neighbours[i, 1], this.cols);

            dataGrid[y, x] = (byte) (dataGrid[y, x] + delta);
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
        Stack<int[]> changeList = new Stack<int[]>();
        for (int row = 0; row < this.rows; row++) {
            for (int col = 0; col < this.cols; col++) {
                byte curr = dataGrid[row, col];
                byte neighbourCount = (byte) (curr >> 1);
                // Dead Cell, no neighbours
                if (curr == 0) continue;

                int[] indices = {row, col};
                if ((curr & 0x01) == 0x01) {
                    if (neighbourCount >= OVERCROWDING_TRESH ||
                        neighbourCount <= ISOLATION_TRESH) {
                        changeList.Push(indices);
                    }
                } else {
                    if (neighbourCount == BIRTH_THRESH) {
                        changeList.Push(indices);
                    }
                }
            }
        }

        while (changeList.Count > 0) {
            int[] curr = changeList.Pop();
            ToggleCell(curr[0], curr[1]);

            grid[curr[0], curr[1]].ToggleState();
        }
        generation++;
        UpdateText();
    }

    /// <summary>
    /// Update the status text area of the simulation with the current
    /// generation and population.
    /// </summary>
    private void UpdateText()
    {
        if (displayText != null) {
            displayText.text = $"Generation {generation}\nPopulation {population}";
        }
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