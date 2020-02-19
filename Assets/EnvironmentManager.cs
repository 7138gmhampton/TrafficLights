using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private enum Corner
    {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }

    private const int UP = 1;
    private const int RIGHT = 2;
    private const int DOWN = 3;
    private const int LEFT = 4;
    private const int JUNCTION = 5;

    public GameObject terrainTile;
    public GameObject roadTile;
    public GameObject junctionTile;

    private readonly int[][] roadLocations = new int[][]
        {
            new int[] { 0,0,1,3,0,0 },
            new int[] { 0,0,1,3,0,0 },
            new int[] { 2,2,5,5,2,2 },
            new int[] { 4,4,5,5,4,4 },
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
                    case LEFT:
                        Instantiate(roadTile, new Vector2(column, row), Quaternion.Euler(0, 0, 270f));
                        break;
                    case RIGHT:
                        Instantiate(roadTile, new Vector2(column, row), Quaternion.Euler(0, 0, 90f));
                        break;
                    case JUNCTION:
                        layJunctionTile(new Vector2(column, row));
                        break;
                    default:
                        Instantiate(terrainTile, new Vector2(column, row), Quaternion.identity);
                        break;
                }
    }

    private void layJunctionTile(Vector2 locus)
    {
        Corner corner = determineCorner(locus);

        switch (corner) {
            case Corner.TOP_LEFT:
                Instantiate(junctionTile, locus, Quaternion.Euler(0, 0, 270));
                break;
            case Corner.TOP_RIGHT:
                Instantiate(junctionTile, locus, Quaternion.Euler(0, 0, 180));
                break;
            case Corner.BOTTOM_LEFT:
                Instantiate(junctionTile, locus, Quaternion.Euler(0, 0, 0));
                break;
            case Corner.BOTTOM_RIGHT:
                Instantiate(junctionTile, locus, Quaternion.Euler(0, 0, 90));
                break;
        }
    }

    private Corner determineCorner(Vector2 locus)
    {
        int above = roadLocations[(int)locus.y + 1][(int)locus.x];
        int after = roadLocations[(int)locus.y][(int)locus.x + 1];
        int below = roadLocations[(int)locus.y - 1][(int)locus.x];
        int before = roadLocations[(int)locus.y][(int)locus.x - 1];

        if (after == 5 && below == 5) return Corner.TOP_LEFT;
        else if (before == 5 && below == 5) return Corner.TOP_RIGHT;
        else if (after == 5) return Corner.BOTTOM_LEFT;
        else return Corner.BOTTOM_RIGHT;
    }
}
