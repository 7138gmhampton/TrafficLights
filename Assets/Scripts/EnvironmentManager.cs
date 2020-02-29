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

    public int xSize;
    public int ySize;
    public GameObject terrainTile;
    public GameObject roadTile;
    public GameObject junctionTile;
    public CarManager carManager;
    public GameObject junctionController;
    public AllLightsController allLightsController;

    private int[,] environment;
    private List<GameObject> junctions = new List<GameObject>();

    private void Awake()
    {
        initialiseEnviroment();
        drawAllRoads();

        layTiles();
        createJunctionControllers();
        allLightsController.junctions = collateJunctions();
    }

    private void Start()
    {
        carManager.XEnd = xSize - 1;
        carManager.YEnd = ySize - 1;
        carManager.placeZones();
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
        var corner = determineCorner(locus);

        switch (corner) {
            case Corner.TOP_LEFT:
                junctions.Add(Instantiate(junctionTile, locus, Quaternion.Euler(0, 0, 270)));
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

    private void createJunctionControllers()
    {
        foreach (var junction in junctions)
            Instantiate(junctionController, junction.transform.position, Quaternion.identity);
    }

    private List<Junction> collateJunctions()
    {
        var allJunctions = new List<Junction>();

        foreach (var junctionController in junctions)
            allJunctions.Add(new Junction(((int)junctionController.transform.position.x) / 6,
                ((int)junctionController.transform.position.y) / 6));

        return allJunctions;
    }
}
