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

    public int xSize;
    public int ySize;
    public GameObject terrainTile;
    public GameObject roadTile;
    public GameObject junctionTile;

    //private readonly int[][] roadLocations = new int[][]
    //    {
    //        new int[] { 0,0,1,3,0,0 },
    //        new int[] { 0,0,1,3,0,0 },
    //        new int[] { 2,2,5,5,2,2 },
    //        new int[] { 4,4,5,5,4,4 },
    //        new int[] { 0,0,1,3,0,0 },
    //        new int[] { 0,0,1,3,0,0 }
    //    };
    private int[,] environment;

    void Awake()
    {
        initialiseEnviroment();

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
        for (int row = 0; row < ySize; ++row)
            for (int column = 0; column < xSize; ++column)
                switch (environment[row,column]) {
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
        int above = environment[(int)locus.y + 1,(int)locus.x];
        int after = environment[(int)locus.y,(int)locus.x + 1];
        int below = environment[(int)locus.y - 1,(int)locus.x];
        int before = environment[(int)locus.y,(int)locus.x - 1];

        if (after == 5 && below == 5) return Corner.TOP_LEFT;
        else if (before == 5 && below == 5) return Corner.TOP_RIGHT;
        else if (after == 5) return Corner.BOTTOM_LEFT;
        else return Corner.BOTTOM_RIGHT;
    }

    private int[,] initialiseEnviroment()
    {
        environment = new int[ySize, xSize];

        for (int row = 0; row < ySize; ++row)
            for (int column = 0; column < xSize; ++column)
                environment[row,column] = 0;

        return environment;
    }
}
