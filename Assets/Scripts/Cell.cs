using UnityEngine;

public class Cell
{
    public bool Alive
    {
        get { return alive; }
    }
    private bool alive;
    private Color liveCol = Color.green;
    private Color deadCol = Color.black;
    private SpriteRenderer spriteRenderer;

    public Cell(Sprite sprite, bool alive, Vector2 position)
    {
        this.alive = alive;

        GameObject g = new GameObject($"X: {position.x}; Y: {position.y}");
        g.transform.position = position;
        spriteRenderer = g.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = alive ? liveCol : deadCol;
    }

    private void UpdateColor()
    {
        spriteRenderer.color = alive ? liveCol : deadCol;
    }

    public void ToggleState()
    {
        alive = !alive;
        UpdateColor();
    }
}