using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameObject car;
    public GameObject spawner;

    private int xStart;
    private int xEnd;
    private int yStart;
    private int yEnd;
    private List<GameObject> spawners = new List<GameObject>();

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

    private void placeSpawnersVertical(int xAxis, int row)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                //var nextSpawner = Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity);
                spawners.Add(Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity));
            }
    }

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
