using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetricsWatcher : MonoBehaviour
{
    public int queueLength;
    public AllLightsController lightsController;

    private Queue<float> journeyTimes = new Queue<float>();

    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(reportJourneyTimeMean());
        //var times = getWaitTimes();
        //Debug.Log(times[0, 0].Item1);
        //Debug.Log(times[0, 0].Item2);
        //Debug.Log(times[0, 0].Item3);
        //Debug.Log(times[0, 0].Item4);
    }

    public void addJourneyTime(float time)
    {
        journeyTimes.Enqueue(time);

        while (journeyTimes.Count > queueLength)
            journeyTimes.Dequeue();
    }

    public float reportJourneyTimeMean()
    {
        if (journeyTimes.Count == 0) return 1000f;

        return journeyTimes.Average();
    }

    private Tuple<float,float,float,float>[,] getWaitTimes()
    {
        int width = lightsController.reportHighestX() + 1;
        int height = lightsController.reportHighestY() + 1;

        var waitTimes = new Tuple<float, float, float, float>[height, width];

        for (int y = 0; y < height; ++y)
            for (int x = 0; x < width; ++x)
                waitTimes[y, x] = lightsController.reportWaitTimes(x, y);
        //float northTime = lightsController.fin

        return waitTimes;
    }
}
