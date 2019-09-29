using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider slider;
    /// <summary>
    /// Reference to the game's grid
    /// </summary>
    private GameGrid grid;
    /// <summary>
    /// Counter used for timing purposes. Increments on each update
    /// </summary>
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null) {
            grid = new GameGrid(sprite);
            if (grid == null) {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }
    }

    /// <summary>
    /// Update is called once per frame. During each update,
    /// the grid is updated.
    /// </summary>
    void Update()
    {
        if (count >= slider?.value) {
            grid?.Update();
            count = 0;
        } else {
            if (slider?.value != slider?.maxValue)
                count++;
        }
    }
}
