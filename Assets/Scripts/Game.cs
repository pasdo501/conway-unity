using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null) {
            GameGrid grid = new GameGrid(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
