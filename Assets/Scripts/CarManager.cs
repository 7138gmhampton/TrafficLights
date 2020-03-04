using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public partial class CarManager : MonoBehaviour
{
    public GameObject car;
    public GameObject spawner;
    public GameObject despawner;

    private int xStart;
    private int xEnd;
    private int yStart;
    private int yEnd;
    private List<GameObject> spawners = new List<GameObject>();
    private List<GameObject> despawners = new List<GameObject>();
    private int maxCars;
    private int noOfCars;
    private List<GameObject> cars = new List<GameObject>();

    public int XStart { set { xStart = value; } }
    public int XEnd { set { xEnd = value; } }
    public int YStart { set { yStart = value; } }
    public int YEnd { set { yEnd = value; } }
    public int MaxCars { set { maxCars = value; } }
    public int NoOfCars { get { return noOfCars; } set { noOfCars = value; } }
    public List<GameObject> Cars { get { return cars; } set { cars = value; } }

    private void Start()
    {
        xStart = 0;
        yStart = 0;
    }

    void Update()
    {
        spawnCar();
        
        if (!isMovementOccuring()) {
            foreach (var eachCar in cars)
                eachCar.GetComponent<Car>().moveCar();
            resolveConflicts();
            foreach (var eachCar in cars)
                eachCar.GetComponent<Car>().doMovement();
        }
    }

    private bool isMovementOccuring()
    {
        return cars.Any(x => x.GetComponent<Car>().Moving);
    }

    private void resolveConflicts()
    {
        for (int iii = 0; iii < cars.Count; ++iii)
            for (int jjj = iii + 1; jjj < cars.Count; ++jjj)
                if (cars[jjj].GetComponent<Car>().NextMovement == cars[iii].GetComponent<Car>().NextMovement)
                    cars[jjj].GetComponent<Car>().setNextMovement(0, 0);
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
        if (cars.Count >= maxCars) return;

        var spawnPoint = selectRandomSpawner();
        if (Physics2D.OverlapBox(spawnPoint, new Vector2(0.4f, 0.4f), 0).gameObject.tag == "Vehicle")
            return;

        var nextCar = Instantiate(car, spawnPoint, Quaternion.identity);
        nextCar.transform.parent = gameObject.transform;
        cars.Add(nextCar);
        ++noOfCars;
    }

    public void removeCar(GameObject car)
    {
        cars.Remove(car);
    }

    private Vector3 selectRandomSpawner()
    {
        var spawnPoint = spawners[Random.Range(0, spawners.Count)];

        return spawnPoint.transform.position;
    }
}
