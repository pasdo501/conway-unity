# Conway's Game of Life (Unity)

Implementation of the famous [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life) simulation in C#, using the Unity game engine.

_The project is currently deployed at https://conwayunity.azurewebsites.net/_

## Implementation Notes

Each cell is represented as a byte, with the top 3 bits unused, and next 4 bits used to store a count of the cell's live neighbours, and the lowest bit being used to store the cell's state (alive = high or dead = low).

i.e - XXXN NNNS

Where X is an unused bit; N is the cell's live neighbour count; S is the cell's state.

This implementation is an adapted version of [Michael Abrash's optimised version of Life](http://www.jagregory.com/abrash-black-book/#chapter-18-its-a-plain-wonderful-life) - see Listing 17.5 L17-5.CPP.

## Optimisation Notes
Profiling of the initial (very simple) implementation suggested 94% of CPU usage was down to the [LiveNeighbourCount function](https://github.com/pasdo501/conway-unity/blob/d9bea95915779066b179f0f81c63499116d2fdc2/Assets/Scripts/GameGrid.cs#L165) (function used to count how many live neighbours a cell has).

This initial implementation resulted in ~20 - 25 generations / second.

The optimised implementation is heavily based on [Michael Abrash's optimised version of the simulation](http://www.jagregory.com/abrash-black-book/#chapter-18-its-a-plain-wonderful-life). While not making use of pointer arithmetic and still using a 2D array with a wrapping index function, CPU usage by the main update loop has dropped dramatically (~11%), with the result now ~60 generations / second (possible limited by Unity's framerate?).

---

### General Notes
- The initial game state is random, and currently cannot be changed in any way. The probability of any given cell being alive at the start of the game is 0.08.
- No stoppage mechanism of any kind is implemented. The simulation will keep going even if there are no more cells alive or a stable equilibrium has been reached.
- Implementation details are found in the [Scripts](./Assets/Scripts) directory.