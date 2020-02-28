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

    public int XStart { set { xStart = value; } }
    public int XEnd { set { xEnd = value; } }
    public int YStart { set { yStart = value; } }
    public int YEnd { set { yEnd = value; } }

    void Start()
    {
        //placeSpawnersAll();
        xStart = 0;
        yStart = 0;
    }

    void Update()
    {
        
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
        for (int x = 0; x < xEnd; ++x)
            if (x % 6 == column) Instantiate(spawner, new Vector3(x, yAxis, 0f), Quaternion.identity);
    }

    private void placeSpawnersVertical(int xAxis, int row)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) Instantiate(spawner, new Vector3(xAxis, y, 0f), Quaternion.identity);
    }
}
