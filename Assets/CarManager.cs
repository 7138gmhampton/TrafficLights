using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarManager : MonoBehaviour
{
    public GameObject car;
    public GameObject spawner;

    private int xStart;
    private int xEnd;
    private int yStart;
    private int yEnd;
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> despawners = new List<GameObject>();
    private static readonly Dictionary<Tuple<string, bool, bool>, Tuple<int, int>> zoneDeploymentRules = 
        new Dictionary<Tuple<string, bool, bool>, Tuple<int, int>>()
    {
            { new Tuple<string, bool, bool>("Spawner", false, false), new Tuple<int, int>(2, 0) },
            { new Tuple<string, bool, bool>("Spawner", false, true), new Tuple<int, int>(3, 180) },
            { new Tuple<string, bool, bool>("Spawner", true, false), new Tuple<int, int>(3, 90) },
            { new Tuple<string, bool, bool>("Spawner", true, true), new Tuple<int, int>(2, 270) },

            { new Tuple<string, bool, bool>("Despawner", false, false), new Tuple<int, int>(3, 0) },
            { new Tuple<string, bool, bool>("Despawner", false, true), new Tuple<int, int>(2, 180) },
            { new Tuple<string, bool, bool>("Despawner", true, false), new Tuple<int, int>(2, 90) },
            { new Tuple<string, bool, bool>("Despawner", true, true), new Tuple<int, int>(3, 270) }
    };

    public int XStart { set { xStart = value; } }
    public int XEnd { set { xEnd = value; } }
    public int YStart { set { yStart = value; } }
    public int YEnd { set { yEnd = value; } }

    void Start()
    {
        //placeSpawnersAll();
        xStart = 0;
        yStart = 0;
        //spawners = new List<GameObject>();
    }

    void Update()
    {
        spawnCar();
        //checkThis();
    }

    public void placeSpawnersAll()
    {
        placeSpawnersHorizontal(0, 2);
        placeSpawnersHorizontal(yEnd, 3);
        placeSpawnersVertical(0, 3);
        placeSpawnersVertical(xEnd, 2);

        //spawnCar();
    }

    private void placeSpawnersHorizontal(int yAxis, int column)
    {
        for (int x = 0; x < xEnd; ++x) {
            //var nextSpawner = spawner;
            //spawners.Add(nextSpawner);
            if (x % 6 == column) {
                var nextSpawner = Instantiate(spawner, new Vector3(x, yAxis, 0f), Quaternion.identity);
                spawners.Add(nextSpawner);
            }
        }
    }

    public void placeZones()
    {
        placeZonesHorizontal(spawner, 0, 2, 0);
    }

    private void placeSpawnersVertical(int xAxis, int row)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                //var nextSpawner = Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity);
                spawners.Add(Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity));
            }
    }

    private void placeZonesHorizontal(GameObject zoneTile, int yAxis, int column, int rotation)
    {
        //if (zoneTile.tag == "Spawners") var 
        //List<GameObject>
        //switch (zoneTile.tag) {
        //    case "Spawner": var collection = spawners; break;
        //    case "Despawner": var collection = despawners; break;
        //}

        for (int x = 0; x < xEnd; ++x)
            if (x % 6 == column) {
                if (zoneTile.tag == "Spawner")
                    spawners.Add(Instantiate(zoneTile, new Vector3(x, yAxis, 0f), Quaternion.Euler(0, 0, rotation)));
                else despawners.Add(Instantiate(zoneTile, new Vector3(x, yAxis, 0f), Quaternion.Euler(0, 0, rotation)));
            }
    }

    private void placeZonesVertical(GameObject zoneTile, int xAxis, int row, int rotation)
    {

    }
    //private void placeZones(GameObject zoneTile, int axialPosition, bool vertical)
    //{
    //    var axisEnd = vertical ? yEnd : xEnd;
    //    var placingInx = zoneDeploymentRules[new Tuple<string, bool, bool>(zoneTile.tag, axialPosition > 0, vertical)];

    //    for (int axis = 0; axis < axisEnd; ++axis)
    //        if (axis % 6 == placingInx.Item1)
    //            switch (zoneTile.tag) {
    //                case "Spawner":
    //                    spawners.Add(Instantiate(zoneTile, new Vector3()))
    //            }
    //}

    private void spawnCar()
    {
        var spawnPoint = selectRandomSpawner();
        //if (Physics.OverlapBox(spawnPoint, new Vector3(0.4f, 0.4f, 0.4f)).Length > 0)
        //    return;
        if (Physics2D.OverlapBox(spawnPoint, new Vector2(0.4f, 0.4f), 0).gameObject.tag == "Movable")
            return;

        Instantiate(car, spawnPoint, Quaternion.identity);
    }

    private void checkThis()
    {
        Debug.Log(Physics.OverlapBox(new Vector3(0, 0, 0), new Vector3(0.4f, 0.4f, 0.4f)).Length);
        Debug.Log(Physics.OverlapBox(new Vector3(-1, -1, 0), new Vector3(0.4f, 0.4f, 0.4f)).Length);
        Debug.Log(Physics2D.OverlapBox(new Vector2(0, 0), new Vector2(0.4f, 0.4f), 0).gameObject.tag);
        Debug.Log(Physics2D.OverlapBox(new Vector2(-1, -1), new Vector2(0.4f, 0.4f), 0).gameObject.tag);
    }

    private Vector3 selectRandomSpawner()
    {
        var spawnPoint = spawners[Random.Range(0, spawners.Count)];

        return spawnPoint.transform.position;
    }
}
