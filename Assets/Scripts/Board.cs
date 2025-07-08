using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public Tilemap tilemap { get; private set; }

    [SerializeField]
    public Tile unknown, empty, mine, mineExploded, flag, one, two, three, four, five, six, seven, eight;

    private int xPos, yPos;

    public int getX() {
        return xPos;
    }

    public int getY() {
        return yPos;
    }

    public void setX(int x) {
        xPos = x;
    }

    public void setY(int y) {
        yPos = y;
    }

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
}