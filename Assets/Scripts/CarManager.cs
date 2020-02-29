using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarManager : MonoBehaviour
{
    private struct Rule
    {
        public Action<GameObject,int,int,int> Method { get; set; }
        public GameObject ZoneTile { get; set; }
        public int Axis { get; set; }
        public int Position { get; set; }
        public int Azimuth { get; set; }

        internal Rule(
            Action<GameObject, int, int, int> method, 
            GameObject zoneTile, 
            int axis,
            int position,
            int azimuth)
        {
            Method = method;
            ZoneTile = zoneTile;
            Axis = axis;
            Position = position;
            Azimuth = azimuth;
        }
    }

    public GameObject car;
    public GameObject spawner;
    public GameObject despawner;

    private int xStart;
    private int xEnd;
    private int yStart;
    private int yEnd;
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> despawners = new List<GameObject>();

    public int XStart { set { xStart = value; } }
    public int XEnd { set { xEnd = value; } }
    public int YStart { set { yStart = value; } }
    public int YEnd { set { yEnd = value; } }

    void Start()
    {
        xStart = 0;
        yStart = 0;
    }

    void Update()
    {
        spawnCar();
    }

    public void placeSpawnersAll()
    {
        placeSpawnersHorizontal(0, 2);
        placeSpawnersHorizontal(yEnd, 3);
        placeSpawnersVertical(0, 3);
        placeSpawnersVertical(xEnd, 2);
    }

    private void placeSpawnersHorizontal(int yAxis, int column)
    {
        for (int x = 0; x < xEnd; ++x) {
            if (x % 6 == column) {
                var nextSpawner = Instantiate(spawner, new Vector3(x, yAxis, 0f), Quaternion.identity);
                spawners.Add(nextSpawner);
            }
        }
    }

    public void placeZones()
    {
        var deploymentRules = new List<Rule>()
        {
            new Rule(placeZonesHorizontal, spawner, 0, 2, 0),
            new Rule(placeZonesHorizontal, spawner, yEnd, 3, 180),
            new Rule(placeZonesVertical, spawner, 0, 3, 270),
            new Rule(placeZonesVertical, spawner, xEnd, 2, 90),

            new Rule(placeZonesHorizontal, despawner, 0, 3, 0),
            new Rule(placeZonesHorizontal, despawner, yEnd, 2, 180),
            new Rule(placeZonesVertical, despawner, 0, 2, 270),
            new Rule(placeZonesVertical, despawner, xEnd, 3, 90)
        };

        foreach (var rule in deploymentRules)
            rule.Method(rule.ZoneTile, rule.Axis, rule.Position, rule.Azimuth);
    }

    private void placeSpawnersVertical(int xAxis, int row)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                spawners.Add(Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity));
            }
    }

    private void placeZonesHorizontal(GameObject zoneTile, int yAxis, int column, int rotation)
    {
        for (int x = 0; x < xEnd; ++x)
            if (x % 6 == column) {
                if (zoneTile.tag == "Spawner")
                    spawners.Add(Instantiate(zoneTile, new Vector3(x, yAxis, 0f), Quaternion.Euler(0, 0, rotation)));
                else despawners.Add(Instantiate(zoneTile, new Vector3(x, yAxis, 0f), Quaternion.Euler(0, 0, rotation)));
            }
    }

    private void placeZonesVertical(GameObject zoneTile, int xAxis, int row, int azimuth)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                var locus = new Vector3(xAxis, y, 0f);
                var rotation = Quaternion.Euler(0, 0, azimuth);
                if (zoneTile.tag == "Spawner")
                    spawners.Add(Instantiate(zoneTile, locus, rotation));
                else despawners.Add(Instantiate(zoneTile, locus, rotation));
            }
    }

    private void spawnCar()
    {
        var spawnPoint = selectRandomSpawner();
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
