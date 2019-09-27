using UnityEngine;

/// <summary>
/// Class representing each individual cell in the grid.
/// </summary>
public class Cell
{
    /// <summary>
    /// Whether the Cell is currently Alive or not.
    /// </summary>
    /// <value><c>Alive</c> represents if the cell is alive</value>
    public bool Alive
    {
        get { return alive; }
    }
    /// <summary>
    /// Instance variable representing if the Cell is alive.
    /// </summary>
    private bool alive;
    /// <summary>
    /// The colour of a live cell.
    /// </summary>
    private Color liveCol = Color.green;
    /// <summary>
    /// The colour of a dead cell.
    /// </summary>
    private Color deadCol = Color.black;
    /// <summary>
    /// Reference to the SpriteRenderer component of the Cell's game object.
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Constructor. Creates a Cell with a given <paramref name="sprite"/>,
    /// <paramref name="position"/>, and life cycle state 
    /// (<paramref name="alive"/>).
    /// </summary>
    /// <param name="sprite">The sprite to use for rendering this cell</param>
    /// <param name="alive">Whether or not the Cell will initially be alive</param>
    /// <param name="position">The cell's position</param>
    public Cell(Sprite sprite, bool alive, Vector2 position)
    {
        this.alive = alive;

        GameObject g = new GameObject("Cell");
        g.transform.position = position;
        spriteRenderer = g.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = alive ? liveCol : deadCol;
    }

    /// <summary>
    /// Updates the Cell's display colour depending on its current state 
    /// (alive or dead).
    /// </summary>
    private void UpdateColor()
    {
        spriteRenderer.color = alive ? liveCol : deadCol;
    }

    /// <summary>
    /// Toggle the Cell's current state (alive/dead).
    /// </summary>
    public void ToggleState()
    {
        alive = !alive;
        UpdateColor();
    }
}