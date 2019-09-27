using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Sprite sprite;
    private GameGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null) {
            grid = new GameGrid(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        grid.Update();
    }
}
