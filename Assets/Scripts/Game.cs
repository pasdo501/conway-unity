using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Game management class. Responsible for getting the initial grid
/// constructed and then asking it to update each frame.
/// </summary>
public class Game : MonoBehaviour
{
    /// <summary>
    /// Sprite component property, assigned in the Unity Editor
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// Reference to the game's grid
    /// </summary>
    private GameGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null) {
            grid = new GameGrid(sprite);
        }
    }

    /// <summary>
    /// Update is called once per frame. During each update,
    /// the grid is updated.
    /// </summary>
    void Update()
    {
        grid.Update();
    }
}
