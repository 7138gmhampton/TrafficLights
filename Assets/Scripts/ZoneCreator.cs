using System;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCreator : MonoBehaviour
{
    private struct Rule
    {
        public Action<GameObject, int, int, int> Method { get; set; }
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

    private GameObject spawner;
    private GameObject despawner;
    private int xEnd;
    private int yEnd;
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
                var zone = Instantiate(zoneTile, new Vector3(x, yAxis, 0f), Quaternion.Euler(0, 0, rotation));
                manager.addZone(zone);
            }
    }

    private void placeZonesVertical(GameObject zoneTile, int xAxis, int row, int azimuth)
    {
        for (int y = 0; y < yEnd; ++y)
            if (y % 6 == row) {
                var locus = new Vector3(xAxis, y, 0f);
                var rotation = Quaternion.Euler(0, 0, azimuth);
                var zone = Instantiate(zoneTile, locus, rotation);
                manager.addZone(zone);
            }
    }
}