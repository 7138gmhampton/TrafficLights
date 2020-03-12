using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MetricsWatcher : MonoBehaviour
{
    public int queueLength;
    public AllLightsController lightsController;
    public string logName;

    private Queue<float> journeyTimes = new Queue<float>();
    private StreamWriter timesLog;

    private void Awake()
    {
        timesLog = new StreamWriter("TimeLog" + logName + ".txt", false);
    }

    private void OnDestroy()
    {
        while (journeyTimes.Count > 0) appendTimeToLog(journeyTimes.Dequeue());
        timesLog.Flush();
        timesLog.Dispose();
    }

    public void addJourneyTime(float time)
    {
        journeyTimes.Enqueue(time);

        while (journeyTimes.Count > queueLength) appendTimeToLog(journeyTimes.Dequeue());
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

    public void resetMetrics() => journeyTimes.Clear();

    private void appendTimeToLog(float time)
    {
        timesLog.WriteLine(time.ToString("0.000"));
    }
}
