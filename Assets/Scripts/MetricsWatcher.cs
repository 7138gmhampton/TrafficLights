using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetricsWatcher : MonoBehaviour
{
    public int queueLength;
    public AllLightsController lightsController;

    private Queue<float> journeyTimes = new Queue<float>();

    public void addJourneyTime(float time)
    {
        journeyTimes.Enqueue(time);

        while (journeyTimes.Count > queueLength) journeyTimes.Dequeue();
    }

    public float reportJourneyTimeMean() => journeyTimes.Count == 0 ? 1000f : journeyTimes.Average();

    public Tuple<float,float,float,float>[,] getWaitTimes()
    {
        int width = lightsController.reportHighestX() + 1;
        int height = lightsController.reportHighestY() + 1;
        var waitTimes = new Tuple<float, float, float, float>[height, width];

        for (int y = 0; y < height; ++y)
            for (int x = 0; x < width; ++x)
                waitTimes[y, x] = lightsController.reportWaitTimes(x, y);

        return waitTimes;
    }
}
