using System.Collections.Generic;
using UnityEngine;

public partial class ZoneCreator : MonoBehaviour
{
    private readonly GameObject spawner;
    private readonly GameObject despawner;
    private readonly int xEnd;
    private readonly int yEnd;
    private CarManager manager;

    public ZoneCreator(GameObject spawner, GameObject despawner, int xEnd, int yEnd, CarManager manager)
    {
        this.spawner = spawner;
        this.despawner = despawner;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        this.manager = manager;
    }

    public void placeZones()
    {
        var deploymentRules = getRules();

        foreach (var rule in deploymentRules)
            rule.Method(rule.ZoneTile, rule.Axis, rule.Position, rule.Azimuth);
    }

    private List<Rule> getRules()
    {
        return new List<Rule>()
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
    }

    private void placeZonesHorizontal(GameObject zoneTile, int yAxis, int column, int rotation)
    {
        for (int x = 0; x < xEnd; ++x)
            if (x % 6 == column) {
                manager.addZone(Instantiate(
                    zoneTile, 
                    new Vector3(x, yAxis, 0f), 
                    Quaternion.Euler(0, 0, rotation)));
            }
    }

    private void placeZonesVertical(GameObject zoneTile, int xAxis, int row, int azimuth)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                manager.addZone(Instantiate(
                    zoneTile, 
                    new Vector3(xAxis, y, 0f), 
                    Quaternion.Euler(0, 0, azimuth)));
            }
    }
}