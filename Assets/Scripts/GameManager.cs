using UnityEngine;

public class GameManager : MonoBehaviour {
    public int width = 16;
    public int height = 16;
    private int mines = 50;
    private bool clickedOnce = false;

    private Board board;
    private Cell[,] state;

    private const int DEBUG = 1;

    private void Awake() {
        board = GetComponentInChildren<Board>();
    }

    private void Start() {
        //Set up Board
        board.CreateBoard(3,3,width, height);

        //Set up Camera
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
    }

    
    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Flag();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (!clickedOnce) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = board.tilemap.WorldToCell(mousePos);
                board.GenerateMines(cellPos.x, cellPos.y, 0,0);
                board.GenerateNumbers();
                clickedOnce = true;
            }

            Click();
        }

        
        if (Input.GetKeyDown("r")) {
            clickedOnce = false;
            board.CreateBoard(0,0,width, height);
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