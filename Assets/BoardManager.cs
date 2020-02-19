using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const int UP = 1;
    private const int RIGHT = 2;
    private const int DOWN = 3;
    private const int LEFT = 4;

    public GameObject terrainTile;
    public GameObject roadTile;

    private readonly int[][] roadLocations = new int[][]
        {
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 }
        };

    void Awake()
    {
        layTiles();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void layTiles()
    {
        for (int row = 0; row < roadLocations.Length; ++row)
            for (int column = 0; column < roadLocations[row].Length; ++column)
                switch (roadLocations[row][column]) {
                    case UP:
                        Instantiate(roadTile, new Vector2(column, row), Quaternion.identity);
                        break;
                    case DOWN:
                        Instantiate(roadTile, new Vector2(column, row), Quaternion.Euler(0,0,180f));
                        break;
                    default:
                        Instantiate(terrainTile, new Vector2(column, row), Quaternion.identity);
                        break;
                }
    }
}
