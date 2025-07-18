using UnityEngine;

public class GameManager : MonoBehaviour {
    public int width = 16;
    public int height = 16;
    private int mines = 50;
    private bool clickedOnce = false;

    private Board board;
    private Cell[,] state;

    private const int DEBUG = 0;

    private void Awake() {
        board = GetComponentInChildren<Board>();
    }

    private void Start() {
        //Set up Board
        CreateBoard(0,0,width, height);
        CreateBoard(16, 0, width, height);

        //Set up Camera
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
    }

    private void CreateBoard(int xPos, int yPos, int width, int height) {
        Cell[,] state;
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

        // Generate Mines
        GenerateMines(xPos, yPos, width, height, state);
        GenerateNumbers(state);
        board.SetTile(state);
    }

    private void GenerateMines(int safeX, int safeY, int xPos, int yPos, Cell[,] state) {
        // For each mine
        for (int i = 0; i < mines; i++) {
            // Generate a random position
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);

            // Skip the cell if it is in the safe zone
            if (x >= safeX - 1 && x <= safeX + 1 && y >= safeY - 1 && y <= safeY + 1) {
                i--;
                continue;
            }

            // Set the cell to be a mine
            if (state[x, y].type != Cell.Type.Mine)
                state[x, y].type = Cell.Type.Mine;
            else i--;

            // Set the cell to be clicked if in debug mode
            if (DEBUG == 1) state[x, y].isClicked = true;
        }
    }

    private void GenerateNumbers(Cell[,] state) {
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

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Flag();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (!clickedOnce) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = board.tilemap.WorldToCell(mousePos);
                //GenerateMines(cellPos.x, cellPos.y, 0,0);
                //GenerateNumbers();
                clickedOnce = true;
            }

            Click();
        }

        if (Input.GetKeyDown("r")) {
            clickedOnce = false;
            CreateBoard(0,0,width, height);
            return;
        }
    }

    private void Flag() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(mousePos);
        state[cellPos.x, cellPos.y].isFlagged = !state[cellPos.x, cellPos.y].isFlagged;
        board.SetTile(state);
    }

    private void Click() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = board.tilemap.WorldToCell(mousePos);
        state[cellPos.x, cellPos.y].isClicked = true;
        if (state[cellPos.x, cellPos.y].type == Cell.Type.Empty) Flood(cellPos.x, cellPos.y);
        board.SetTile(state);
    }

    private void Flood(int x, int y) {
        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                if (x + i < 0 || x + i >= width || y + j < 0 || y + j >= height) continue;
                if (state[x + i, y + j].type == Cell.Type.Number) state[x + i, y + j].isClicked = true;
                if (state[x + i, y + j].type == Cell.Type.Empty && !state[x + i, y + j].isClicked) {
                    state[x + i, y + j].isClicked = true;
                    Flood(x + i, y + j);
                }
            }
        }
    }
}