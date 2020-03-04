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

    public int XStart { set { xStart = value; } }
    public int XEnd { set { xEnd = value; } }
    public int YStart { set { yStart = value; } }
    public int YEnd { set { yEnd = value; } }
    public int MaxCars { set { maxCars = value; } }
    public List<GameObject> Cars { get; set; } = new List<GameObject>();

    private void Start()
    {
        xStart = 0;
        yStart = 0;
    }

    private void Update()
    {
        spawnCar();
        if (!isMovementOccuring()) moveCars();
    }

    private void moveCars()
    {
        foreach (var eachCar in Cars) eachCar.GetComponent<Car>().moveCar();
        resolveConflicts();

        foreach (var eachCar in Cars) eachCar.GetComponent<Car>().doMovement();
    }

    private bool isMovementOccuring() => Cars.Any(x => x.GetComponent<Car>().Moving);

    private void resolveConflicts()
    {
        for (int iii = 0; iii < Cars.Count; ++iii)
            for (int jjj = iii + 1; jjj < Cars.Count; ++jjj)
                if (Cars[jjj].GetComponent<Car>().NextMovement == Cars[iii].GetComponent<Car>().NextMovement)
                    Cars[jjj].GetComponent<Car>().setNextMovement(0, 0);
    }

    public void addZone(GameObject zone)
    {
        if (zone.tag == "Spawner") spawners.Add(zone);
        else despawners.Add(zone);
    }

    public void placeZones()
    {
        var zoneCreator = new ZoneCreator(spawner, despawner, xEnd, yEnd, this);

        zoneCreator.placeZones();
    }

    private void spawnCar()
    {
        if (Cars.Count >= maxCars) return;

        var spawnPoint = selectRandomSpawner();
        if (Physics2D.OverlapBox(spawnPoint, new Vector2(0.4f, 0.4f), 0).gameObject.tag == "Vehicle")
            return;

        var nextCar = Instantiate(car, spawnPoint, Quaternion.identity);
        nextCar.transform.parent = gameObject.transform;

        Cars.Add(nextCar);
    }

    public void removeCar(GameObject car) => Cars.Remove(car);

    private Vector3 selectRandomSpawner() => 
        spawners[Random.Range(0, spawners.Count)].transform.position;
}
