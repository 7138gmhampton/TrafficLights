using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public partial class CarManager : MonoBehaviour
{
    public GameObject car;
    public GameObject spawner;
    public GameObject despawner;
    public float unacceptableWaitTime;
    public LayerMask blockingLayer;

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

    public void resetCars()
    {
        for (int iii = 0; iii < Cars.Count; iii++) {
            var car = Cars[iii];
            Destroy(car);
        }

        Cars.Clear();
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
            for (int jjj = iii + 1; jjj < Cars.Count; ++jjj) {
                var firstCar = Cars[iii].GetComponent<Car>();
                var secondCar = Cars[jjj].GetComponent<Car>();

                if (secondCar.NextMovement == firstCar.NextMovement)
                    secondCar.setNextMovement(0, 0);
            }
    }

    public void addZone(GameObject zone)
    {
        if (zone.tag == "Spawner") spawners.Add(zone);
        else despawners.Add(zone);
    }

    public void placeZones() => new ZoneCreator(spawner, despawner, xEnd, yEnd, this).placeZones();

    private void spawnCar()
    {
        if (Cars.Count >= maxCars) return;

        var spawnPoint = selectRandomSpawner();
        //if (Physics2D.OverlapBox(spawnPoint, new Vector2(0.4f, 0.4f), 0, blockingLayer).gameObject.tag == "Vehicle")
        //    return;
        if (isBlocked(spawnPoint)) return;

        var nextCar = Instantiate(car, spawnPoint, Quaternion.identity);
        nextCar.transform.parent = gameObject.transform;
        nextCar.GetComponent<Car>().UnacceptableWaitTime = unacceptableWaitTime;

        Cars.Add(nextCar);
    }

    public void removeCar(GameObject car) => Cars.Remove(car);

    private Vector3 selectRandomSpawner() => 
        spawners[Random.Range(0, spawners.Count)].transform.position;

    private bool isBlocked(Vector2 locus)
    {
        var collider = Physics2D.OverlapPoint(locus, blockingLayer);

        if (collider != null) return true;
        else return false;
    }
}
