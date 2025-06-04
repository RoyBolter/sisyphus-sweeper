using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public Tilemap tilemap { get; private set; }

    [SerializeField]
    public Tile unknown, empty, mine, mineExploded, flag, one, two, three, four, five, six, seven, eight;
    
    private Cell[,] state;
    public Board board;
    
    public int width;
    public int height;
    public int xPos;
    public int yPos;
    public int mines;
    
    private const int DEBUG = 0;

    private void Awake() {
        tilemap = GetComponent<Tilemap>();
    }

    public void SetTile(Cell[,] state) {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }

    private Tile GetTile(Cell cell) {
        if (cell.isClicked) {
            switch (cell.type) {
                case Cell.Type.Empty: return empty;
                case Cell.Type.Mine: return mineExploded;
                case Cell.Type.Number: return GetNumberTile(cell);
                default: return empty;
            }
        }

        if (cell.isFlagged) return flag;
        return unknown;
    }

    private Tile GetNumberTile(Cell cell) {
        switch (cell.number) {
            case 1: return one;
            case 2: return two;
            case 3: return three;
            case 4: return four;
            case 5: return five;
            case 6: return six;
            case 7: return seven;
            case 8: return eight;
            default: return null;
        }
    }
    
    public void CreateBoard(int xPos, int yPos, int width, int height) {
        state = new Cell[width, height];

        // Initialize Board
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x + xPos, y + yPos, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;

                if (DEBUG == 1) state[x, y].isClicked = true;
            }
        }

        board.SetTile(state);
    }

    public void GenerateMines(int safeX, int safeY, int xPos, int yPos) {
        for (int i = 0; i < mines; i++) {
            int x = UnityEngine.Random.Range(0, width) + xPos;
            int y = UnityEngine.Random.Range(0, height) + yPos;

            if (x >= safeX - 1 && x <= safeX + 1 && y >= safeY - 1 && y <= safeY + 1) {
                i--;
                continue;
            }

            if (state[x, y].type != Cell.Type.Mine)
                state[x, y].type = Cell.Type.Mine;
            else i--;

            if (DEBUG == 1) state[x, y].isClicked = true;
        }
    }

    public void GenerateNumbers() {
        // For each cell
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // Skip the cell if it is a mine
                if (state[x, y].type == Cell.Type.Mine) continue;
                // Count the number of mines around the cell
                int count = 0;
                for (int i = -1; i < 2; i++) {
                    for (int j = -1; j < 2; j++) {
                        if (x + i < 0 || x + i >= width || y + j < 0 || y + j >= height) continue;
                        if (state[x + i, y + j].type == Cell.Type.Mine) count++;
                    }
                }

                // Set the number of mines around the cell
                state[x, y].type = Cell.Type.Number;
                state[x, y].number = count;

                // Make empty tiles empty
                if (count == 0) state[x, y].type = Cell.Type.Empty;

                if (DEBUG == 1) state[x, y].isClicked = true;
            }
        }
    }
}