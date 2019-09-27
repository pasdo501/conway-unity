# Conway's Game of Life (Unity)

Implementation of the famous [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) simulation in C#, using the Unity game engine.

## Notes
- The initial game state is random, and currently cannot be changed in any way. The probability of any given cell being alive at the start of the game is 0.2.
- No stoppage mechanism of any kind is implemented. The simulation will keep going even if there are no more cells alive or a stable equilibrium has been reached.
- Implementation details are found in the `Assets/Scripts` directory.