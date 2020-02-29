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

    private const int TERRAIN = 0;
    private const int UP = 6;
    private const int RIGHT = 7;
    private const int DOWN = 8;
    private const int LEFT = 9;
    //private const int JUNCTION = 5;

    public int xSize;
    public int ySize;
    public GameObject terrainTile;
    public GameObject roadTile;
    public GameObject junctionTile;
    public CarManager carManager;

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
        drawAllRoads();

        layTiles();
        //Debug.Log(environment[3, 3].ToString());
        //Debug.Log(environment[4, 3].ToString());
        carManager.XEnd = xSize - 1;
        carManager.YEnd = ySize - 1;
        //carManager.placeSpawnersAll();
        carManager.placeZones();
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
                    //case JUNCTION:
                    //    layJunctionTile(new Vector2(column, row));
                    //    break;
                    case TERRAIN:
                        Instantiate(terrainTile, new Vector2(column, row), Quaternion.identity);
                        break;
                    default:
                        layJunctionTile(new Vector2(column, row));
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

        if (after > LEFT && below > LEFT) return Corner.TOP_LEFT;
        else if (before > LEFT  && below > LEFT) return Corner.TOP_RIGHT;
        else if (after > LEFT) return Corner.BOTTOM_LEFT;
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

    private void drawAllRoads()
    {
        for (int column = 0; column < xSize; ++column)
            if ((column + 1) % 6 == 0) drawRoad(drawVerticalRoadSide, column - 3, UP);

        for (int row = 0; row < ySize; ++row)
            if ((row + 1) % 6 == 0) drawRoad(drawHorizontalRoadSide, row - 3, RIGHT);
    }

    //private void drawVerticalRoad(int upColumn)
    //{
    //    drawVerticalRoadSide(upColumn, UP);
    //    drawVerticalRoadSide(upColumn + 1, DOWN);
    //}

    private void drawRoad(System.Action<int, int> drawFunction, int earlierIndex, int firstDirection)
    {
        drawFunction(earlierIndex, firstDirection);
        drawFunction(earlierIndex + 1, firstDirection + 2);
    }

    private void drawVerticalRoadSide(int column, int direction)
    {
        for (int row = 0; row < ySize; ++row)
            environment[row, column] += direction;
    }

    private void drawHorizontalRoadSide(int row, int direction)
    {
        for (int column = 0; column < xSize; ++column)
            environment[row, column] += direction;
    }
}
