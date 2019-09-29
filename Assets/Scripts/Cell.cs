using UnityEngine;

/// <summary>
/// Class representing each individual cell in the grid.
/// </summary>
public class Cell
{   
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
        

        GameObject g = new GameObject("Cell");
        g.transform.position = position;
        spriteRenderer = g.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = Color.green;
        spriteRenderer.enabled = alive;
    }

    /// <summary>
    /// Toggle the Cell's current state (alive/dead).
    /// </summary>
    public void ToggleState()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }
}