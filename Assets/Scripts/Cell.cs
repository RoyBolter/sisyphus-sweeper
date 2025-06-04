using UnityEngine;

public struct Cell {
    public enum Type {
        Empty,
        Mine,
        Number,
    }

    public Vector3Int position;
    public Type type;
    public int number;
    public bool isClicked;
    public bool isFlagged;
    public bool isExploded;
}